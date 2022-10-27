using System;
using System.IO;

namespace Linear;

/// <summary>
/// Adds multiple buffers for reducing reads to underlying stream
/// </summary>
public class MultiBufferStream : Stream
{
    private readonly Stream _sourceStream;
    private readonly CircleBuffer<(long chunk, int length, byte[] buffer)> _buffers;
    private readonly int _bufferCount;
    private readonly int _bufferLength;
    private long _position;

    private bool _disposed;

    /// <summary>
    /// Override large reads, don't proxy through buffers
    /// </summary>
    public bool LargeReadOverride = true;

    /// <summary>
    /// Create new instance of <see cref="MultiBufferStream"/>
    /// </summary>
    /// <param name="sourceStream">Stream to wrap</param>
    /// <param name="bufferCount">Number of buffers to use</param>
    /// <param name="bufferLength">Individual buffer length</param>
    public MultiBufferStream(Stream sourceStream, int bufferCount = 32,
        int bufferLength = 4096)
    {
        if (!sourceStream.CanSeek)
            throw new ArgumentException($"Cannot create {nameof(MultiBufferStream)} with non-seekable stream");
        if (!sourceStream.CanRead)
            throw new ArgumentException($"Cannot create {nameof(MultiBufferStream)} with non-readable stream");
        _sourceStream = sourceStream;
        _bufferCount = bufferCount;
        _bufferLength = bufferLength;
        _buffers = new CircleBuffer<(long chunk, int length, byte[] buffer)>(bufferCount);
        _position = 0;
        _disposed = false;
    }

    private ArraySegment<byte> GetOrRead(long position, int length)
    {
        long chunk = position / _bufferLength;
        int ofs = (int)(position % _bufferLength);
        (long chunk, int length, byte[] buffer) target;
        for (int i = 0; i < _buffers.Count; i++)
        {
            target = _buffers[i];
            if (target.chunk == chunk)
            {
                return ofs > target.length
                    ? new ArraySegment<byte>()
                    : new ArraySegment<byte>(target.buffer,ofs, Math.Min(target.length - ofs, length));
            }
        }

        // Read from stream
        if (_buffers.Count != _bufferCount)
        {
            // Allocate new
            byte[] newBuf = new byte[_bufferLength];
            target = ForceRead(chunk, newBuf);
        }
        else
        {
            // Reuse last existing
            (long chunk, int length, byte[] buffer) last = _buffers[_bufferCount - 1];
            _buffers.RemoveAt(_bufferCount - 1);
            target = ForceRead(chunk, last.buffer);
        }

        // Add target to circle
        _buffers.Insert(0, target);

        return ofs > target.length
            ? new ArraySegment<byte>()
            : new ArraySegment<byte>(target.buffer,ofs, Math.Min(target.length - ofs, length));
    }

    private (long chunk, int length, byte[] buffer) ForceRead(long chunk, byte[] buffer)
    {
        _sourceStream.Position = chunk * _bufferLength;
        int read = ReadBaseArray(_sourceStream, buffer, 0, _bufferLength, true);
        return (chunk, read, buffer);
    }

    private static int ReadBaseArray(Stream stream, byte[] array, int offset, int length, bool lenient)
    {
        int left = length, read, tot = 0;
        do
        {
            read = stream.Read(array, offset + tot, left);
            left -= read;
            tot += read;
        } while (left > 0 && read != 0);

        if (left > 0 && read == 0 && !lenient)
        {
            throw new ApplicationException(
                $"Failed to read required number of bytes! 0x{read:X} read, 0x{left:X} left, 0x{stream.Position:X} end position");
        }

        return tot;
    }

    /// <inheritdoc />
    public override void Flush()
    {
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (count > _bufferLength && LargeReadOverride)
        {
            _sourceStream.Position = _position;
            int srcRead = _sourceStream.Read(buffer, offset, count);
            _position = srcRead;
            return srcRead;
        }

        int read = 0;
        while (count > 0)
        {
            ArraySegment<byte> source = GetOrRead(_position, Math.Min(count, buffer.Length - offset));
            int r = source.Count;
            if (r == 0)
                return read;
            Array.Copy(source.Array, source.Offset, buffer, offset, r);
            count -= r;
            read += r;
            offset += r;
            _position += r;
        }

        return read;
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        return _position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _position + offset,
            SeekOrigin.End => _sourceStream.Length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin))
        };
    }

    /// <inheritdoc />
    public override void SetLength(long value) => throw new NotSupportedException();

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanSeek => true;

    /// <inheritdoc />
    public override bool CanWrite => false;

    /// <inheritdoc />
    public override long Length => _sourceStream.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => _position;
        set => _position = value;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;
        base.Dispose(disposing);
    }
}
