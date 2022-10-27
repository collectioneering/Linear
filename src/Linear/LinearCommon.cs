using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Linear.Runtime;
using Linear.Runtime.Deserializers;
using Linear.Runtime.Exporters;
using Linear.Runtime.Expressions;

namespace Linear;

/// <summary>
/// Lyn processing utility
/// </summary>
public static class LinearCommon
{
    /// <summary>
    /// Preferred name for primary structure
    /// </summary>
    public const string MainLayout = "main";

    private static readonly bool _little = BitConverter.IsLittleEndian;

    /*internal const string ArrayLengthProperty = "array_length";
    internal const string PointerArrayLengthProperty = "pointer_array_length";
    internal const string PointerOffsetProperty = "pointer_offset";*/

    /// <summary>
    /// Standard properties
    /// </summary>
    public enum StandardProperty
    {
        /// <summary>
        /// Array length
        /// </summary>
        ArrayLengthProperty,

        /// <summary>
        /// Pointer length
        /// </summary>
        PointerArrayLengthProperty,

        /// <summary>
        /// Pointer offset
        /// </summary>
        PointerOffsetProperty
    }

    /// <summary>
    /// Generate processor
    /// </summary>
    /// <param name="input">Lyn format reader</param>
    /// <param name="registry">Generated registry</param>
    /// <param name="logDelegate">Log delegate</param>
    /// <param name="deserializers">Custom deserializers to use</param>
    /// <param name="methods">Custom expression methods to use</param>
    /// <param name="errorHandler">Parser error handler</param>
    /// <returns>True if succeeded</returns>
    public static bool TryGenerateRegistry(TextReader input, out StructureRegistry? registry, Action<string> logDelegate,
        IReadOnlyCollection<IDeserializer>? deserializers = null,
        IReadOnlyCollection<(string, MethodCallExpression.MethodCallDelegate)>? methods = null,
        IAntlrErrorStrategy? errorHandler = null)
    {
        var inputStream = new AntlrInputStream(input);
        var lexer = new LinearLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new LinearParser(tokens);

        var listenerPre = new LinearPreListener();
        if (errorHandler != null)
            parser.ErrorHandler = errorHandler;
        ParseTreeWalker.Default.Walk(listenerPre, parser.compilation_unit());
        if (listenerPre.Fail)
        {
            registry = null;
            return false;
        }

        Dictionary<string, IDeserializer> r_deserializers = CreateDefaultDeserializerRegistry();
        if (deserializers != null)
        {
            foreach (var deserializer in deserializers)
            {
                string? dname = deserializer.GetTargetTypeName();
                if (dname == null)
                {
                    logDelegate( "Target name is required for all user-defined deserializers");
                    registry = null;
                    return false;
                }
                r_deserializers[dname] = deserializer;
            }
        }

        Dictionary<string, MethodCallExpression.MethodCallDelegate> r_methods = CreateDefaultMethodDictionary();
        if (methods != null)
        {
            foreach (var method in methods)
            {
                r_methods[method.Item1] = method.Item2;
            }
        }

        foreach (string name in listenerPre.GetStructureNames())
            r_deserializers[name] = new StructureDeserializer(name);
        var listener = new LinearListener(r_deserializers, r_methods, logDelegate);
        parser.Reset();
        ParseTreeWalker.Default.Walk(listener, parser.compilation_unit());
        List<StructureDefinition> structures = listener.GetStructures();
        registry = new StructureRegistry();
        foreach (StructureDefinition structure in structures)
        {
            registry.Add(structure.Build());
        }

        return true;
    }

    private static readonly Dictionary<string, MethodCallExpression.MethodCallDelegate> _defaultMethods =
        new Dictionary<string, MethodCallExpression.MethodCallDelegate>
        {
            {
                "log", args =>
                {
                    string? value = args[0]?.ToString();
                    Console.WriteLine(value);
                    return args[0];
                }
            },
            {"format", args => string.Format(args[0]?.ToString() ?? "", args.Skip(1).ToArray())}
        };

    /// <summary>
    /// Create default method dictionary with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, MethodCallExpression.MethodCallDelegate> CreateDefaultMethodDictionary()
    {
        return new Dictionary<string, MethodCallExpression.MethodCallDelegate>(_defaultMethods);
    }

    private static readonly Dictionary<string, IExporter> _defaultExporters = new Dictionary<string, IExporter>
    {
        {DataExporter.ExporterName, new DataExporter()},
        {DecompressExporter.ExporterName, new DecompressExporter()}
    };

    /// <summary>
    /// Create default exporter dictionary with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, IExporter> CreateDefaultExporterDictionary()
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static short Reverse(short value)
    {
        return (short)((value & 0x00FF) << 8 | (value & 0xFF00) >> 8);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Reverse(int value) => (int)Reverse((uint)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long Reverse(long value) => (long)Reverse((ulong)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ushort Reverse(ushort value)
    {
        return (ushort)((value << 8) + (value >> 8));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Reverse(uint value)
    {
        uint mask_xx_zz = value & 0x00FF00FFU;
        uint mask_ww_yy = value & 0xFF00FF00U;
        return ((mask_xx_zz >> 8) | (mask_xx_zz << 24))
               + ((mask_ww_yy << 8) | (mask_ww_yy >> 24));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Reverse(ulong value)
    {
        return ((ulong)Reverse((uint)value) << 32)
               + Reverse((uint)(value >> 32));
    }

    internal static unsafe bool ReadBool(Stream stream, long offset, byte[]? tempBuffer)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[1];
        ReadBase(stream, tempBuffer, 0, 1);
        fixed (void* p = tempBuffer)
            return *(bool*)p;
    }

    internal static unsafe byte ReadU8(Stream stream, long offset, byte[]? tempBuffer)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[1];
        ReadBase(stream, tempBuffer, 0, 1);
        fixed (void* p = tempBuffer)
            return *(byte*)p;
    }

    internal static unsafe sbyte ReadS8(Stream stream, long offset, byte[]? tempBuffer)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[1];
        ReadBase(stream, tempBuffer, 0, 1);
        fixed (void* p = tempBuffer)
            return *(sbyte*)p;
    }

    internal static unsafe ushort ReadU16(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[2];
        ReadBase(stream, tempBuffer, 0, 2);
        fixed (void* p = tempBuffer)
            return littleEndian ^ _little ? Reverse(*(ushort*)p) : *(ushort*)p;
    }

    internal static unsafe short ReadS16(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[2];
        ReadBase(stream, tempBuffer, 0, 2);
        fixed (void* p = tempBuffer)
            return littleEndian ^ _little ? Reverse(*(short*)p) : *(short*)p;
    }

    internal static unsafe uint ReadU32(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[4];
        ReadBase(stream, tempBuffer, 0, 4);
        fixed (void* p = tempBuffer)
            return littleEndian ^ _little ? Reverse(*(uint*)p) : *(uint*)p;
    }

    internal static unsafe int ReadS32(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[4];
        ReadBase(stream, tempBuffer, 0, 4);
        fixed (void* p = tempBuffer)
            return littleEndian ^ _little ? Reverse(*(int*)p) : *(int*)p;
    }

    internal static unsafe ulong ReadU64(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[8];
        ReadBase(stream, tempBuffer, 0, 8);
        fixed (void* p = tempBuffer)
            return littleEndian ^ _little ? Reverse(*(ulong*)p) : *(ulong*)p;
    }

    internal static unsafe long ReadS64(Stream stream, long offset, byte[]? tempBuffer, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[8];
        ReadBase(stream, tempBuffer, 0, 8);
        fixed (void* p = tempBuffer)
            return littleEndian ^ _little ? Reverse(*(long*)p) : *(long*)p;
    }

    internal static unsafe float ReadSingle(Stream stream, long offset, byte[]? tempBuffer)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[4];
        ReadBase(stream, tempBuffer, 0, 4);
        fixed (void* p = tempBuffer)
            return *(float*)p;
    }

    internal static unsafe double ReadDouble(Stream stream, long offset, byte[]? tempBuffer)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        tempBuffer ??= new byte[8];
        ReadBase(stream, tempBuffer, 0, 8);
        fixed (void* p = tempBuffer)
            return *(double*)p;
    }

    private static void ReadBase(Stream stream, byte[] buffer, int offset, int length)
    {
        int left = length, read, tot = 0;
        do
        {
            read = stream.Read(buffer, offset + tot, left);
            left -= read;
            tot += read;
        } while (left > 0 && read != 0);

        if (left > 0)
            throw new EndOfStreamException(
                $"Failed to read required number of bytes! 0x{read:X} read, 0x{left:X} left, 0x{stream.Position:X} end position");
    }
}
