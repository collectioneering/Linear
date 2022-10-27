using System;
using System.Collections.Generic;
using System.IO;
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

        private static Encoding GetUtf16Encoding(bool bigEndian, bool bom) =>
            GUtf16Encodings[(bigEndian ? 1 : 0) + (bom ? 2 : 0)];

        private static Encoding[] GUtf16Encodings => _gUtf16Encodings ??= new Encoding[] { new UnicodeEncoding(false, false), new UnicodeEncoding(true, false), new UnicodeEncoding(false, true), new UnicodeEncoding(true, true) };

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
        public DeserializeResult Deserialize(StructureInstance instance, Stream stream,
            long offset, bool littleEndian, Dictionary<LinearCommon.StandardProperty, object>? standardProperties,
            Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            stream.Position = instance.AbsoluteOffset + offset;
            switch (_mode)
            {
                case Mode.Utf8Fixed:
                    {
                        (string item1, int item2) = ReadUtf8String(stream, (int)length);
                        if (length != item2)
                            throw new Exception(
                                $"UTF-8 fixed length mismatch between specified length {length} and result length {item2}");
                        return new DeserializeResult(item1, item2);
                    }
                case Mode.Utf8Null:
                    {
                        (string item1, int item2) = ReadUtf8String(stream);
                        return new DeserializeResult(item1, item2 + 1);
                    }
                case Mode.Utf16Fixed:
                    {
                        (string item1, int item2) = ReadUtf16String(stream, (int)length);
                        if (length != item2)
                            throw new Exception(
                                $"UTF-16 fixed length mismatch between specified length {length} and result length {item2}");
                        return new DeserializeResult(item1, item2);
                    }
                case Mode.Utf16Null:
                    {
                        (string item1, int item2) = ReadUtf16String(stream);
                        return new DeserializeResult(item1, item2 + 2);
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private (string, int) ReadUtf8String(Stream stream, int maxLength = int.MaxValue)
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

        private static string ReadUtf8String(ArraySegment<byte> segment, int maxLength = int.MaxValue)
        {
            // TODO switch to ROS impl
            int lim = Math.Min(segment.Count, maxLength);

            int end = Array.IndexOf(segment.Array, 0, segment.Offset, lim) - segment.Offset;
            if (end == -1)
            {
                end = lim;
            }

            return DecodeSegment(new ArraySegment<byte>(segment.Array, segment.Offset, end), Encoding.UTF8);
        }

        private (string, int) ReadUtf16String(Stream stream, int maxLength = int.MaxValue / 2)
        {
            try
            {
                TempMs.Position = 0;
                TempMs.SetLength(0);
                int c = 0;
                while (c < maxLength * 2)
                {
                    int cc = ReadBaseArray(stream, _tempBuffer, 0, 2);
                    c += cc;
                    if (cc != 2 || _tempBuffer[0] == 0 && _tempBuffer[1] == 0)
                    {
                        break;
                    }

                    TempMs.Write(_tempBuffer, 0, 2);
                }
                // WARNING: maxLength here is treated as # code units

                TempMs.TryGetBuffer(out ArraySegment<byte> buffer);
                return (ReadUtf16String(buffer), c);
            }
            finally
            {
                if (TempMs.Capacity > StringExcessiveCapacity)
                {
                    TempMs.Capacity = StringDefaultCapacity;
                }
            }
        }

        private static string DecodeSegment(ArraySegment<byte> segment, Encoding encoding)
        {
            return segment.Count == 0 ? string.Empty : encoding.GetString(segment.Array, segment.Offset, segment.Count);
        }

        private static unsafe string ReadUtf16String(ArraySegment<byte> segment, int maxLength = int.MaxValue)
        {
            int lim = Math.Min(segment.Count, maxLength);
            int end = -1;
            byte[] array = segment.Array;
            int offset = segment.Offset;
            int count = segment.Count;
            fixed (byte* p = array)
            {
                char* c = (char*)(p + offset);
                for (int i = 0; i < lim; i++)
                    if (c[i] == '\0')
                    {
                        end = i;
                        break;
                    }
            }

            if (end == -1)
            {
                end = lim;
            }
            else
            {
                end *= sizeof(char);
            }

            bool big = count >= 2 && array[offset + 0] == 0xFE && array[offset + 1] == 0xFF;
            bool bom = big || count >= 2 && array[offset + 0] == 0xFF && array[offset + 1] == 0xFE;

            if (!bom && count > 1)
            {
                const int numBytes = 16 * sizeof(char);
                const float threshold = 0.75f;
                int countAscii = 0, countTotal = 0;
                for (int i = 0; i < numBytes && i + 1 < count; i += 2)
                {
                    if (array[offset + i] == 0 && array[offset + i + 1] < 0x80)
                    {
                        countAscii++;
                    }

                    countTotal++;
                }

                big = (float)countAscii / countTotal >= threshold;
            }

            return DecodeSegment(new ArraySegment<byte>(array, offset, end), GetUtf16Encoding(big, bom));
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

            return tot;
        }
    }
}
