using System;
using System.IO;

namespace Linear
{
    /// <summary>
    /// Stream that acts as a limited-range proxy for another stream
    /// </summary>
    public class SStream : Stream
    {
        private Stream _sourceStream = null!;
        private long _offset;
        private long _position;
        private long _length;

        /// <summary>
        /// Create a default empty instance of <see cref="SStream"/>
        /// </summary>
        /// <param name="isolate">If true, enforces stream position for object before reads</param>
        public SStream(bool isolate = false)
        {
            Set(Null, 0, isolate);
        }

        /// <summary>
        /// Create a new instance of <see cref="SStream"/>
        /// </summary>
        /// <param name="sourceStream">Stream to wrap</param>
        /// <param name="length">Length of proxy</param>
        /// <param name="isolate">If true, enforces stream position for object before reads</param>
        public SStream(Stream sourceStream, long length, bool isolate = false)
        {
            Set(sourceStream, length, isolate);
        }

        /// <summary>
        /// Create a new instance of <see cref="SStream"/>
        /// </summary>
        /// <param name="sourceStream">Stream to wrap</param>
        /// <param name="offset">Offset of proxy</param>
        /// <param name="length">Length of proxy</param>
        /// <param name="isolate">If true, enforces stream position for object before reads</param>
        public SStream(Stream sourceStream, long offset, long length, bool isolate = false)
        {
            Set(sourceStream, offset, length, isolate);
        }

        /// <summary>
        /// If true, enforces stream position for object before reads
        /// </summary>
        public bool Isolate;

        /// <inheritdoc />
        public override bool CanRead => _sourceStream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => _sourceStream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => _sourceStream.CanWrite;

        /// <inheritdoc />
        public override long Length => _length;

        /// <inheritdoc />
        public override long Position
        {
            get => CanSeek ? _position : throw new NotSupportedException();
            set => _position = CanSeek ? value : throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override void Flush() => _sourceStream.Flush();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (Isolate && _offset + _position != _sourceStream.Position)
                _sourceStream.Seek(_offset + _position, SeekOrigin.Begin);
            int read = _sourceStream.Read(buffer, offset,
                (int)(Math.Min(_length, _position + count) - _position));
            _position += read;
            return read;
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!CanSeek) throw new NotSupportedException();
            if (!Isolate && origin == SeekOrigin.Current)
                return _position = _sourceStream.Seek(offset, SeekOrigin.Begin) - _offset;
            return _position = origin switch
            {
                SeekOrigin.Begin => _sourceStream.Seek(_offset + offset, SeekOrigin.Begin),
                SeekOrigin.Current => _sourceStream.Seek(_offset + _position + offset, SeekOrigin.Begin),
                SeekOrigin.End => _sourceStream.Seek(_offset + _length - _sourceStream.Length + offset, SeekOrigin.End),
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            } - _offset;
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            if (value < 0) throw new ArgumentException("Length cannot be negative");
            if (_sourceStream.CanSeek && _offset + _length > _sourceStream.Length)
                throw new ArgumentException(
                    $"Cannot set length to {value}, base stream length {_sourceStream.Length} self offset {_offset}");
            _length = value;
            _position = Math.Min(_length, _position);
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (Isolate && _offset + _position != _sourceStream.Position)
                _sourceStream.Seek(_offset + _position, SeekOrigin.Begin);
            int write = (int)(Math.Min(_length, _position + count) - _position);
            _sourceStream.Write(buffer, offset, write);
            _position += write;
        }

        /// <summary>
        /// Set source for this stream
        /// </summary>
        /// <param name="stream">Source stream</param>
        /// <param name="length">Source stream length</param>
        /// <param name="isolate">If true, enforces stream position for object before reads, no change if null</param>
        public void Set(Stream stream, long length, bool? isolate = true)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (isolate.HasValue)
            {
                if (isolate.Value && !stream.CanSeek)
                    throw new ArgumentException("Cannot set isolate if stream is not seekable");
                Isolate = isolate.Value;
            }

            _sourceStream = stream;
            _offset = _sourceStream.CanSeek ? _sourceStream.Position : 0;
            _length = length;
            _position = 0;
        }

        /// <summary>
        /// Set source for this stream
        /// </summary>
        /// <param name="stream">Source stream</param>
        /// <param name="offset">Source stream offset</param>
        /// <param name="length">Source stream length</param>
        /// <param name="isolate">If true, enforces stream position for object before reads, no change if null</param>
        public void Set(Stream stream, long offset, long length, bool? isolate = true)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanSeek)
                throw new ArgumentException(
                    $"Cannot set stream with parameter {nameof(offset)} if stream is not seekable");
            if (isolate.HasValue)
                Isolate = isolate.Value;
            _sourceStream = stream;
            _offset = offset;
            if (!Isolate && _sourceStream.CanSeek)
                _sourceStream.Position = _offset;
            _length = length;
            _position = 0;
        }
    }
}
