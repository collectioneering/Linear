using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Linear.Runtime;
using Linear.Runtime.Deserializers;
using Linear.Runtime.Exporters;
using static System.Buffers.ArrayPool<byte>;

namespace Linear
{
    /// <summary>
    /// Lyn processing utility
    /// </summary>
    public static class LinearUtil
    {
        /// <summary>
        /// Preferred name for primary structure
        /// </summary>
        public const string MainLayout = "main";

        internal const string ArrayLengthProperty = "array_length";
        internal const string PointerArrayLengthProperty = "pointer_array_length";
        internal const string PointerOffsetProperty = "pointer_offset";

        /// <summary>
        /// Generate processor
        /// </summary>
        /// <param name="input">Lyn format stream</param>
        /// <param name="deserializers">Custom deserializers to use</param>
        public static StructureRegistry GenerateRegistry(Stream input,
            IReadOnlyCollection<IDeserializer>? deserializers = null)
        {
            var inputStream = new AntlrInputStream(input);
            var lexer = new LinearLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new LinearParser(tokens);

            var listenerPre = new LinearPreListener();
            parser.ErrorHandler = new BailErrorStrategy();
            ParseTreeWalker.Default.Walk(listenerPre, parser.compilation_unit());
            Dictionary<string, IDeserializer> r_deserializers = CreateDefaultDeserializerRegistry();
            if (deserializers != null)
            {
                foreach (var deserializer in deserializers)
                {
                    string dname =
                        deserializer.GetTargetTypeName() ??
                        throw new NullReferenceException("Target name is required for user-defined deserializers");
                    r_deserializers[dname] = deserializer;
                }
            }

            foreach (string name in listenerPre.GetStructureNames())
                r_deserializers[name] = new StructureDeserializer(name);
            var listener = new LinearListener(r_deserializers);
            parser.Reset();
            ParseTreeWalker.Default.Walk(listener, parser.compilation_unit());
            List<StructureDefinition> structures = listener.GetStructures();
            StructureRegistry registry = new StructureRegistry();
            foreach (StructureDefinition structure in structures)
            {
                registry.Add(structure.Build());
            }

            return registry;
        }

        private static readonly Dictionary<string, IExporter> _defaultExporters = new Dictionary<string, IExporter>
        {
            {DataExporter.DataExporterName, new DataExporter()}
        };

        /// <summary>
        /// Create default exporter registry with standard exporters
        /// </summary>
        /// <returns>Default registry</returns>
        public static Dictionary<string, IExporter> CreateDefaultExporterRegistry()
        {
            return new Dictionary<string, IExporter>(_defaultExporters);
        }

        private static readonly Dictionary<string, IDeserializer> _defaultDeserializers =
            new Dictionary<string, IDeserializer>
            {
                {"byte", new PrimitiveDeserializer(typeof(byte))},
                {"sbyte", new PrimitiveDeserializer(typeof(sbyte))},
                {"ushort", new PrimitiveDeserializer(typeof(ushort))},
                {"short", new PrimitiveDeserializer(typeof(short))},
                {"uint", new PrimitiveDeserializer(typeof(uint))},
                {"int", new PrimitiveDeserializer(typeof(int))},
                {"ulong", new PrimitiveDeserializer(typeof(ulong))},
                {"long", new PrimitiveDeserializer(typeof(long))},
                {"byteb", new PrimitiveDeserializer(typeof(byte))},
                {"sbyteb", new PrimitiveDeserializer(typeof(sbyte))},
                {"ushortb", new PrimitiveDeserializer(typeof(ushort))},
                {"shortb", new PrimitiveDeserializer(typeof(short))},
                {"uintb", new PrimitiveDeserializer(typeof(uint))},
                {"intb", new PrimitiveDeserializer(typeof(int))},
                {"ulongb", new PrimitiveDeserializer(typeof(ulong))},
                {"longb", new PrimitiveDeserializer(typeof(long))},
                {"float", new PrimitiveDeserializer(typeof(float))},
                {"double", new PrimitiveDeserializer(typeof(double))},
                {"string", new StringDeserializer(StringDeserializer.Mode.Utf8Fixed)},
                {"cstring", new StringDeserializer(StringDeserializer.Mode.Utf8Null)},
                {"string16", new StringDeserializer(StringDeserializer.Mode.Utf16Fixed)},
                {"cstring16", new StringDeserializer(StringDeserializer.Mode.Utf16Null)},
            };

        /// <summary>
        /// Create default deserializer registry with standard exporters
        /// </summary>
        /// <returns>Default registry</returns>
        public static Dictionary<string, IDeserializer> CreateDefaultDeserializerRegistry()
        {
            return new Dictionary<string, IDeserializer>(_defaultDeserializers);
        }

        internal static bool TryCast<T>(object? o, out T result)
        {
            if (o is T t)
            {
                result = t;
                return true;
            }

            result = default!;
            return false;
        }

        internal static byte CastByte(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => (byte)b,
                ushort b => (byte)b,
                short b => (byte)b,
                uint b => (byte)b,
                int b => (byte)b,
                ulong b => (byte)b,
                long b => (byte)b,
                float b => (byte)b,
                double b => (byte)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static sbyte CastSByte(object? number)
        {
            return number switch
            {
                byte b => (sbyte)b,
                sbyte b => b,
                ushort b => (sbyte)b,
                short b => (sbyte)b,
                uint b => (sbyte)b,
                int b => (sbyte)b,
                ulong b => (sbyte)b,
                long b => (sbyte)b,
                float b => (sbyte)b,
                double b => (sbyte)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static ushort CastUShort(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => (ushort)b,
                ushort b => b,
                short b => (ushort)b,
                uint b => (ushort)b,
                int b => (ushort)b,
                ulong b => (ushort)b,
                long b => (ushort)b,
                float b => (ushort)b,
                double b => (ushort)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static short CastShort(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => b,
                ushort b => (short)b,
                short b => b,
                uint b => (short)b,
                int b => (short)b,
                ulong b => (short)b,
                long b => (short)b,
                float b => (short)b,
                double b => (short)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static uint CastUInt(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => (uint)b,
                ushort b => b,
                short b => (uint)b,
                uint b => b,
                int b => (uint)b,
                ulong b => (uint)b,
                long b => (uint)b,
                float b => (uint)b,
                double b => (uint)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static int CastInt(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => b,
                ushort b => b,
                short b => b,
                uint b => (int)b,
                int b => b,
                ulong b => (int)b,
                long b => (int)b,
                float b => (int)b,
                double b => (int)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static ulong CastULong(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => (ulong)b,
                ushort b => b,
                short b => (ulong)b,
                uint b => b,
                int b => (ulong)b,
                ulong b => b,
                long b => (ulong)b,
                float b => (ulong)b,
                double b => (ulong)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static long CastLong(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => b,
                ushort b => b,
                short b => b,
                uint b => b,
                int b => b,
                ulong b => (long)b,
                long b => b,
                float b => (long)b,
                double b => (long)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static float CastFloat(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => b,
                ushort b => b,
                short b => b,
                uint b => b,
                int b => b,
                ulong b => b,
                long b => b,
                float b => b,
                double b => (float)b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static double CastDouble(object? number)
        {
            return number switch
            {
                byte b => b,
                sbyte b => b,
                ushort b => b,
                short b => b,
                uint b => b,
                int b => b,
                ulong b => b,
                long b => b,
                float b => b,
                double b => b,
                _ => throw new InvalidCastException(
                    $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
            };
        }

        internal static bool TryCastLong(object? number, out long value)
        {
            (long item1, bool item2) = number switch
            {
                byte b => (b, true),
                sbyte b => (b, true),
                ushort b => (b, true),
                short b => (b, true),
                uint b => (b, true),
                int b => (b, true),
                ulong b => ((long)b, true),
                long b => (b, true),
                float b => ((long)b, true),
                double b => ((long)b, true),
                _ => (0, false)
            };
            value = item1;
            return item2;
        }

        internal static bool ReadBool(Stream stream, long offset, byte[]? tempBuffer)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[1];
            ReadBaseSpan(stream, span, tempBuffer);
            return span[0] != 0;
        }

        internal static byte ReadU8(Stream stream, long offset, byte[]? tempBuffer)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[1];
            ReadBaseSpan(stream, span, tempBuffer);
            return span[0];
        }

        internal static sbyte ReadS8(Stream stream, long offset, byte[]? tempBuffer)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[1];
            ReadBaseSpan(stream, span, tempBuffer);
            return (sbyte)span[0];
        }

        internal static ushort ReadU16(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[2];
            ReadBaseSpan(stream, span, tempBuffer);
            return littleEndian
                ? BinaryPrimitives.ReadUInt16LittleEndian(span)
                : BinaryPrimitives.ReadUInt16BigEndian(span);
        }

        internal static short ReadS16(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[2];
            ReadBaseSpan(stream, span, tempBuffer);
            return littleEndian
                ? BinaryPrimitives.ReadInt16LittleEndian(span)
                : BinaryPrimitives.ReadInt16BigEndian(span);
        }

        internal static uint ReadU32(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[4];
            ReadBaseSpan(stream, span, tempBuffer);
            return littleEndian
                ? BinaryPrimitives.ReadUInt32LittleEndian(span)
                : BinaryPrimitives.ReadUInt32BigEndian(span);
        }

        internal static int ReadS32(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[4];
            ReadBaseSpan(stream, span, tempBuffer);
            return littleEndian
                ? BinaryPrimitives.ReadInt32LittleEndian(span)
                : BinaryPrimitives.ReadInt32BigEndian(span);
        }

        internal static ulong ReadU64(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[8];
            ReadBaseSpan(stream, span, tempBuffer);
            return littleEndian
                ? BinaryPrimitives.ReadUInt64LittleEndian(span)
                : BinaryPrimitives.ReadUInt64BigEndian(span);
        }

        internal static long ReadS64(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[8];
            ReadBaseSpan(stream, span, tempBuffer);
            return littleEndian
                ? BinaryPrimitives.ReadInt64LittleEndian(span)
                : BinaryPrimitives.ReadInt64BigEndian(span);
        }

        internal static float ReadSingle(Stream stream, long offset, byte[]? tempBuffer)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[4];
            ReadBaseSpan(stream, span, tempBuffer);
            return MemoryMarshal.Cast<byte, float>(span)[0];
        }

        internal static double ReadDouble(Stream stream, long offset, byte[]? tempBuffer)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            Span<byte> span = stackalloc byte[8];
            ReadBaseSpan(stream, span, tempBuffer);
            return MemoryMarshal.Cast<byte, double>(span)[0];
        }

        private static void ReadBaseSpan(Stream stream, Span<byte> span, byte[]? tempBuffer)
        {
            var buf = (span.Length <= sizeof(long) ? tempBuffer : null) ?? Shared.Rent(4096);
            Span<byte> bufSpan = buf.AsSpan();
            int bufLen = buf.Length;
            try
            {
                int left = span.Length, read, tot = 0;
                do
                {
                    read = stream.Read(buf, 0, Math.Min(left, bufLen));
                    bufSpan.Slice(0, read).CopyTo(span.Slice(tot));
                    left -= read;
                    tot += read;
                } while (left > 0 && read != 0);

                if (left > 0)
                    throw new EndOfStreamException(
                        $"Failed to read required number of bytes! 0x{read:X} read, 0x{left:X} left, 0x{stream.Position:X} end position");
            }
            finally
            {
                if (buf != tempBuffer)
                {
                    Shared.Return(buf);
                }
            }
        }
    }
}
