using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Fp;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Generic string deserializer
/// </summary>
public class StringDeserializer : IDeserializer
{
    private const int StringDefaultCapacity = 4 * 1024;
    private const int StringExcessiveCapacity = 128 * 1024;

    private static Encoding GetUtf16Encoding(bool bigEndian, bool bom) =>
        GUtf16Encodings[(bigEndian ? 1 : 0) + (bom ? 2 : 0)];

    private static Encoding[] GUtf16Encodings => _gUtf16Encodings ??= new Encoding[] { new UnicodeEncoding(false, false), new UnicodeEncoding(true, false), new UnicodeEncoding(false, true), new UnicodeEncoding(true, true) };

    private static Encoding[]? _gUtf16Encodings;

    private MemoryStream TempMs => _tempMs ??= new MemoryStream();
    private MemoryStream? _tempMs;


    /// <summary>
    /// Deserializer mode
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Fixed-length UTF-8
        /// </summary>
        Utf8Fixed,

        /// <summary>
        /// Null-terminated UTF-8
        /// </summary>
        Utf8Null,

        /// <summary>
        /// Fixed-length UTF-16
        /// </summary>
        Utf16Fixed,

        /// <summary>
        /// Null-terminated UTF-16
        /// </summary>
        Utf16Null
    }

    private readonly Mode _mode;

    /// <summary>
    /// Create new instance of <see cref="StringDeserializer"/>
    /// </summary>
    /// <param name="mode">Deserializer mode</param>
    public StringDeserializer(Mode mode)
    {
        _mode = mode;
    }

    /// <inheritdoc />
    public string? GetTargetTypeName() => null;

    /// <inheritdoc />
    public Type GetTargetType() => typeof(string);

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, Stream stream,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        offset += context.Structure.AbsoluteOffset;
        stream.Position = offset;
        switch (_mode)
        {
            case Mode.Utf8Fixed:
                {
                    if (length is not { } l) throw new InvalidOperationException("No length specified for fixed-length UTF-8 string");
                    var result = ReadUtf8String(stream, (int)l);
                    if (l != result.ByteLength)
                        throw new Exception($"UTF-8 fixed length mismatch between specified length {l} and result length {result.ByteLength}");
                    return new DeserializeResult(result.Text, result.ByteLength);
                }
            case Mode.Utf8Null:
                {
                    var result = ReadUtf8String(stream);
                    return new DeserializeResult(result.Text, result.ByteLength + 1);
                }
            case Mode.Utf16Fixed:
                {
                    if (length is not { } l) throw new InvalidOperationException("No length specified for fixed-length UTF-16 string");
                    var result = ReadUtf16String(stream, (int)l);
                    if (l != result.ByteLength)
                        throw new Exception($"UTF-16 fixed length mismatch between specified length {l} and result length {result.ByteLength}");
                    return new DeserializeResult(result.Text, result.ByteLength);
                }
            case Mode.Utf16Null:
                {
                    var result = ReadUtf16String(stream);
                    return new DeserializeResult(result.Text, result.ByteLength + 2);
                }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        return Deserialize(context, memory.Span, offset, littleEndian, parameters, length, index);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        offset += context.Structure.AbsoluteOffset;
        checked
        {
            span = span[(int)offset..];
        }
        switch (_mode)
        {
            case Mode.Utf8Fixed:
                {
                    if (length is not { } l) throw new InvalidOperationException("No length specified for fixed-length UTF-8 string");
                    string result = Processor.ReadUtf8String(span, out _, out int numBytes, (int)l);
                    if (l != numBytes)
                        throw new Exception($"UTF-8 fixed length mismatch between specified length {l} and result length {numBytes}");
                    return new DeserializeResult(result, numBytes);
                }
            case Mode.Utf8Null:
                {
                    string result = Processor.ReadUtf8String(span, out int read, out _);
                    return new DeserializeResult(result, read);
                }
            case Mode.Utf16Fixed:
                {
                    if (length is not { } l) throw new InvalidOperationException("No length specified for fixed-length UTF-16 string");
                    string result = Processor.ReadUtf16String(span, out _, out int numBytes, (int)l);
                    if (l != numBytes)
                        throw new Exception($"UTF-16 fixed length mismatch between specified length {l} and result length {numBytes}");
                    return new DeserializeResult(result, numBytes);
                }
            case Mode.Utf16Null:
                {
                    string result = Processor.ReadUtf16String(span, out int read, out _);
                    return new DeserializeResult(result, read);
                }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private TextResult ReadUtf8String(Stream stream, int maxLength = int.MaxValue)
    {
        try
        {
            TempMs.Position = 0;
            TempMs.SetLength(0);
            int c = 0;
            while (c < maxLength)
            {
                int v = stream.ReadByte();
                if (v == -1 || v == 0)
                {
                    break;
                }

                TempMs.WriteByte((byte)v);
                c++;
            }

            TempMs.TryGetBuffer(out ArraySegment<byte> buffer);
            string str = ReadUtf8String(buffer);

            return new TextResult(str, c);
        }
        finally
        {
            if (TempMs.Capacity > StringExcessiveCapacity)
            {
                TempMs.Capacity = StringDefaultCapacity;
            }
        }
    }

    private static string ReadUtf8String(ReadOnlySpan<byte> segment, int maxLength = int.MaxValue)
    {
        int lim = Math.Min(segment.Length, maxLength);

        int end = segment[lim..].IndexOf((byte)0);
        if (end == -1)
        {
            end = lim;
        }

        return DecodeSegment(segment[..end], Encoding.UTF8);
    }

    private TextResult ReadUtf16String(Stream stream, int maxLength = int.MaxValue >> 1)
    {
        try
        {
            TempMs.Position = 0;
            TempMs.SetLength(0);
            int c = 0;
            Span<byte> temp = stackalloc byte[2];
            while (c < maxLength * 2)
            {
                int cc = Processor.Read(stream, temp);
                c += cc;
                if (cc != 2 || temp[0] == 0 && temp[1] == 0)
                {
                    break;
                }

                TempMs.Write(temp);
            }
            // WARNING: maxLength here is treated as # code units

            TempMs.TryGetBuffer(out ArraySegment<byte> buffer);
            return new TextResult(ReadUtf16String(buffer), c);
        }
        finally
        {
            if (TempMs.Capacity > StringExcessiveCapacity)
            {
                TempMs.Capacity = StringDefaultCapacity;
            }
        }
    }

    private static string DecodeSegment(ReadOnlySpan<byte> segment, Encoding encoding)
    {
        return segment.Length == 0 ? string.Empty : encoding.GetString(segment);
    }

    private static unsafe string ReadUtf16String(ReadOnlySpan<byte> segment, int maxLengthChars = int.MaxValue)
    {
        int limBytes = Math.Min(segment.Length, maxLengthChars * sizeof(char));
        int endBytes = -1;
        fixed (byte* p = segment)
        {
            char* c = (char*)p;
            int limChars = limBytes >> 1;
            for (int i = 0; i < limChars; i++)
                if (c[i] == '\0')
                {
                    endBytes = i * sizeof(char);
                    break;
                }
        }

        if (endBytes == -1)
        {
            endBytes = limBytes;
        }

        bool big;
        bool bom;
        if (segment.Length >= 2)
        {
            byte b0 = segment[0], b1 = segment[1];
            big = b0 == 0xFE && b1 == 0xFF;
            bom = big || b0 == 0xFF && b1 == 0xFE;
        }
        else
        {
            big = false;
            bom = false;
        }

        if (!bom && limBytes >= 2)
        {
            const int numBytes = 16 * sizeof(char);
            const float threshold = 0.75f;
            int countAscii = 0, countTotal = 0;
            for (int i = 0; i < numBytes && i + 1 < limBytes; i += 2)
            {
                if (segment[i] == 0 && segment[i + 1] < 0x80)
                {
                    countAscii++;
                }

                countTotal++;
            }

            big = (float)countAscii / countTotal >= threshold;
        }

        return DecodeSegment(segment[..endBytes], GetUtf16Encoding(big, bom));
    }

    private readonly record struct TextResult(string Text, int ByteLength);
}
