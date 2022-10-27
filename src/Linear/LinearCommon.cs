using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Fp;
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
        IReadOnlyCollection<MethodCallExpression.NamedDelegate>? methods = null,
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

        Dictionary<string, IDeserializer> rDeserializers = CreateDefaultDeserializerRegistry();
        if (deserializers != null)
        {
            foreach (var deserializer in deserializers)
            {
                string? dname = deserializer.GetTargetTypeName();
                if (dname == null)
                {
                    logDelegate("Target name is required for all user-defined deserializers");
                    registry = null;
                    return false;
                }
                rDeserializers[dname] = deserializer;
            }
        }

        Dictionary<string, MethodCallExpression.MethodCallDelegate> rMethods = CreateDefaultMethodDictionary();
        if (methods != null)
        {
            foreach (var method in methods)
            {
                rMethods[method.Name] = method.Delegate;
            }
        }

        foreach (string name in listenerPre.GetStructureNames())
            rDeserializers[name] = new StructureDeserializer(name);
        var listener = new LinearListener(rDeserializers, rMethods, logDelegate);
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

    private static readonly Dictionary<string, MethodCallExpression.MethodCallDelegate> s_defaultMethods = new() { { "log", Log }, { "format", Format } };

    private static object? Log(params object?[] args)
    {
        string? value = args[0]?.ToString();
        Console.WriteLine(value);
        return args[0];
    }

    private static object Format(params object?[] args)
    {
        return string.Format(args[0]?.ToString() ?? "", args.Skip(1).ToArray());
    }

    /// <summary>
    /// Create default method dictionary with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, MethodCallExpression.MethodCallDelegate> CreateDefaultMethodDictionary()
    {
        return new Dictionary<string, MethodCallExpression.MethodCallDelegate>(s_defaultMethods);
    }

    private static readonly Dictionary<string, IExporter> s_defaultExporters = new Dictionary<string, IExporter> { { DataExporter.ExporterName, new DataExporter() }, { DecompressExporter.ExporterName, new DecompressExporter() } };

    /// <summary>
    /// Create default exporter dictionary with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, IExporter> CreateDefaultExporterDictionary()
    {
        return new Dictionary<string, IExporter>(s_defaultExporters);
    }

    private static readonly Dictionary<string, IDeserializer> s_defaultDeserializers =
        new Dictionary<string, IDeserializer>
        {
            { "byte", new PrimitiveDeserializer(typeof(byte)) },
            { "sbyte", new PrimitiveDeserializer(typeof(sbyte)) },
            { "ushort", new PrimitiveDeserializer(typeof(ushort)) },
            { "short", new PrimitiveDeserializer(typeof(short)) },
            { "uint", new PrimitiveDeserializer(typeof(uint)) },
            { "int", new PrimitiveDeserializer(typeof(int)) },
            { "ulong", new PrimitiveDeserializer(typeof(ulong)) },
            { "long", new PrimitiveDeserializer(typeof(long)) },
            { "byteb", new PrimitiveDeserializer(typeof(byte)) },
            { "sbyteb", new PrimitiveDeserializer(typeof(sbyte)) },
            { "ushortb", new PrimitiveDeserializer(typeof(ushort)) },
            { "shortb", new PrimitiveDeserializer(typeof(short)) },
            { "uintb", new PrimitiveDeserializer(typeof(uint)) },
            { "intb", new PrimitiveDeserializer(typeof(int)) },
            { "ulongb", new PrimitiveDeserializer(typeof(ulong)) },
            { "longb", new PrimitiveDeserializer(typeof(long)) },
            { "float", new PrimitiveDeserializer(typeof(float)) },
            { "double", new PrimitiveDeserializer(typeof(double)) },
            { "string", new StringDeserializer(StringDeserializer.Mode.Utf8Fixed) },
            { "cstring", new StringDeserializer(StringDeserializer.Mode.Utf8Null) },
            { "string16", new StringDeserializer(StringDeserializer.Mode.Utf16Fixed) },
            { "cstring16", new StringDeserializer(StringDeserializer.Mode.Utf16Null) },
        };

    /// <summary>
    /// Create default deserializer registry with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, IDeserializer> CreateDefaultDeserializerRegistry()
    {
        return new Dictionary<string, IDeserializer>(s_defaultDeserializers);
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

    internal static unsafe bool ReadBool(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        Processor.Read(stream, temp, false);
        return temp[0] != 0;
    }

    internal static unsafe byte ReadU8(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        Processor.Read(stream, temp, false);
        return temp[0];
    }

    internal static unsafe sbyte ReadS8(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        Processor.Read(stream, temp, false);
        return (sbyte)temp[0];
    }

    internal static unsafe ushort ReadU16(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[2];
        Processor.Read(stream, temp, false);
        return Processor.GetU16(temp, littleEndian);
    }

    internal static unsafe short ReadS16(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[2];
        Processor.Read(stream, temp, false);
        return Processor.GetS16(temp, littleEndian);
    }

    internal static unsafe uint ReadU32(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        Processor.Read(stream, temp, false);
        return Processor.GetU32(temp, littleEndian);
    }

    internal static unsafe int ReadS32(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        Processor.Read(stream, temp, false);
        return Processor.GetS32(temp, littleEndian);
    }

    internal static unsafe ulong ReadU64(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        Processor.Read(stream, temp, false);
        return Processor.GetU64(temp, littleEndian);
    }

    internal static unsafe long ReadS64(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        Processor.Read(stream, temp, false);
        return Processor.GetS64(temp, littleEndian);
    }

    internal static unsafe float ReadSingle(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        Processor.Read(stream, temp, false);
        return Processor.GetSingle(temp);
    }

    internal static unsafe double ReadDouble(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        Processor.Read(stream, temp, false);
        return Processor.GetDouble(temp);
    }
}
