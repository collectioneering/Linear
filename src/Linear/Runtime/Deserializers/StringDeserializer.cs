using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Linear.Runtime.Deserializers
{
    /// <summary>
    /// Generic string deserializer
    /// </summary>
    public class StringDeserializer : IDeserializer
    {
        private const int StringDefaultCapacity = 4 * 1024;
        private const int StringExcessiveCapacity = 128 * 1024;

        /*private Encoder[] Utf16Encoders => _utf16Encoders ??= new Encoder[GUtf16Encodings.Length];
        private Encoder[]? _utf16Encoders;

        private Encoder GetUtf16Encoder(bool bigEndian, bool bom)
        {
            int i = (bigEndian ? 1 : 0) + (bom ? 2 : 0);
            return Utf16Encoders[i] ??= GUtf16Encodings[i].GetEncoder();
        }*/

        private static Encoding GetUtf16Encoding(bool bigEndian, bool bom) =>
            GUtf16Encodings[(bigEndian ? 1 : 0) + (bom ? 2 : 0)];

        private static Encoding[] GUtf16Encodings => _gUtf16Encodings ??= new Encoding[]
        {
            new UnicodeEncoding(false, false), new UnicodeEncoding(true, false), new UnicodeEncoding(false, true),
            new UnicodeEncoding(true, true)
        };

        private static Encoding[]? _gUtf16Encodings;

        private readonly byte[] _tempBuffer = new byte[sizeof(long)];
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
        public (object value, long length) Deserialize(StructureInstance instance, Stream stream, byte[] tempBuffer,
            long offset, bool littleEndian,Dictionary<LinearUtil.StandardProperty, object>? standardProperties, Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            stream.Position = instance.AbsoluteOffset + offset;
            switch (_mode)
            {
                case Mode.Utf8Fixed:
                {
                    (string item1, int item2) = ReadUtf8String(stream, (int)length);
                    if(length != item2) throw new Exception($"UTF-8 fixed length mismatch between specified length {length} and result length {item2}");
                    return (item1, item2);
                }
                case Mode.Utf8Null:
                {
                    (string item1, int item2) = ReadUtf8String(stream);
                    return (item1, item2 + 1);
                }
                case Mode.Utf16Fixed:
                {
                    (string item1, int item2) = ReadUtf16String(stream, (int)length);
                    if(length != item2) throw new Exception($"UTF-16 fixed length mismatch between specified length {length} and result length {item2}");
                    return (item1, item2);
                }
                case Mode.Utf16Null:
                {
                    (string item1, int item2) = ReadUtf8String(stream);
                    return (item1, item2 + 2);
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private (string, int) ReadUtf8String(Stream stream, int maxLength = int.MaxValue)
        {
            try
            {
                int c = 0;
                do
                {
                    int v = stream.ReadByte();
                    if (v == -1 || v == 0)
                    {
                        break;
                    }

                    TempMs.WriteByte((byte)v);
                    c++;
                } while (c < maxLength);

                string str = ReadUtf8String(TempMs.GetBuffer().AsSpan(0, (int)TempMs.Length));

                return (str, c);
            }
            finally
            {
                if (TempMs.Capacity > StringExcessiveCapacity)
                {
                    TempMs.Capacity = StringDefaultCapacity;
                }
            }
        }

        private static string ReadUtf8String(ReadOnlySpan<byte> span, int maxLength = int.MaxValue)
        {
            int lim = Math.Min(span.Length, maxLength);
            int end = span.Slice(0, lim).IndexOf((byte)0);
            if (end == -1)
            {
                end = lim;
            }

            return DecodeSpan(span.Slice(0, end), Encoding.UTF8);
        }

        private (string, int) ReadUtf16String(Stream stream, int maxLength = int.MaxValue)
        {
            try
            {
                int c = 0;
                do
                {
                    int cc = ReadBaseArray(stream, _tempBuffer, 0, 2);
                    c += cc;
                    if (cc != 2 || _tempBuffer[0] == 0 && _tempBuffer[1] == 0)
                    {
                        break;
                    }

                    TempMs.Write(_tempBuffer, 0, 2);
                } while (c < maxLength * 2);
                // WARNING: maxLength here is treated as # code units

                return (ReadUtf16String(TempMs.GetBuffer().AsSpan(0, (int)TempMs.Length)), c);
            }
            finally
            {
                if (TempMs.Capacity > StringExcessiveCapacity)
                {
                    TempMs.Capacity = StringDefaultCapacity;
                }
            }
        }

        private static unsafe string DecodeSpan(ReadOnlySpan<byte> span, Encoding encoding)
        {
            if (span.Length == 0)
            {
                return string.Empty;
            }

            fixed (byte* spanFixed = &span.GetPinnableReference())
            {
                return encoding.GetString(spanFixed, span.Length);
            }
        }

        private static string ReadUtf16String(ReadOnlySpan<byte> span, int maxLength = int.MaxValue)
        {
            int lim = Math.Min(span.Length, maxLength);
            int end = MemoryMarshal.Cast<byte, char>(span.Slice(0, lim)).IndexOf('\0');
            if (end == -1)
            {
                end = lim;
            }
            else
            {
                end *= sizeof(char);
            }

            bool big = span.Length >= 2 && span[0] == 0xFE && span[1] == 0xFF;
            bool bom = big || span.Length >= 2 && span[0] == 0xFF && span[1] == 0xFE;

            if (!bom && span.Length > 1)
            {
                const int numBytes = 16 * sizeof(char);
                const float threshold = 0.75f;
                int countAscii = 0, countTotal = 0, sl = span.Length;
                for (int i = 0; i < numBytes && i + 1 < sl; i += 2)
                {
                    if (span[i] == 0 && span[i + 1] < 0x80)
                    {
                        countAscii++;
                    }

                    countTotal++;
                }

                big = (float)countAscii / countTotal >= threshold;
            }

            return DecodeSpan(span.Slice(0, end), GetUtf16Encoding(big, bom));
        }

        private static int ReadBaseArray(Stream stream, byte[] array, int offset, int length)
        {
            int left = length, read, tot = 0;
            do
            {
                read = stream.Read(array, offset + tot, left);
                left -= read;
                tot += read;
            } while (left > 0 && read != 0);

            if (left > 0 && read == 0)
            {
                throw new Exception(
                    $"Failed to read required number of bytes! 0x{read:X} read, 0x{left:X} left, 0x{stream.Position:X} end position");
            }

            return tot;
        }
    }
}
