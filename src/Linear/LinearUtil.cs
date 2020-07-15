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

        /// <summary>
        /// Generate processor
        /// </summary>
        /// <param name="input">Lyn format stream</param>
        /// <param name="deserializers">Custom deserializers to use</param>
        public static StructureRegistry GenerateRegistry(Stream input, IReadOnlyCollection<IDeserializer>? deserializers = null)
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
                    r_deserializers[deserializer.GetTargetTypeName()] = deserializer;
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
            new Dictionary<string, IDeserializer> { };

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
