using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Linear.Runtime;
using Linear.Runtime.Deserializers;
using Linear.Runtime.Exporters;
using Linear.Runtime.Expressions;

namespace Linear.Utility;

/// <summary>
/// Base utilities.
/// </summary>
public static class LinearUtil
{
    private static readonly Dictionary<string, MethodCallDelegate> s_defaultMethods = new()
    {
        { "log", Log }, //
        { "format", Format }, //
        { "and", And }, //
        { "or", Or }, //
        { "xor", Xor } //
    };

    private static object? Log(StructureEvaluationContext context, params object?[] args)
    {
        string? value = args[0]?.ToString();
        Console.WriteLine(value);
        return args[0];
    }

    private static object Format(StructureEvaluationContext context, params object?[] args)
    {
        return string.Format(CultureInfo.InvariantCulture, args[0]?.ToString() ?? "", args.Skip(1).ToArray());
    }

    private static object And(StructureEvaluationContext context, params object?[] args)
    {
        switch (args.Length)
        {
            case 2:
                // <buf> <lambda|key>
                if (TryGetReadOnlyMemoryFromPossibleBuffer(args[0], out var buf))
                {
                    if (args[1] is ExpressionInstance expr)
                    {
                        byte[] copied = buf.ToArray();
                        Dictionary<string, object> lambdaReplacements = new();
                        context = context with { LambdaReplacements = lambdaReplacements };
                        for (int i = 0; i < copied.Length; i++)
                        {
                            lambdaReplacements["i"] = i;
                            lambdaReplacements["v"] = copied[i];
                            copied[i] &= CastUtil.CastByte(expr.Evaluate(context, ReadOnlySpan<byte>.Empty));
                        }
                        return new ReadOnlyMemory<byte>(copied);
                    }
                    else
                    {
                        byte key = CastUtil.CastByte(args[1]);
                        byte[] copied = buf.ToArray();
                        for (int i = 0; i < copied.Length; i++)
                            copied[i] &= key;
                        return new ReadOnlyMemory<byte>(copied);
                    }
                }
                throw new ArgumentException("Expected buffer at pos 0");
            default:
                throw new ArgumentException("Invalid method signature");
        }
    }

    private static object Or(StructureEvaluationContext context, params object?[] args)
    {
        switch (args.Length)
        {
            case 2:
                // <buf> <lambda|key>
                if (TryGetReadOnlyMemoryFromPossibleBuffer(args[0], out var buf))
                {
                    if (args[1] is ExpressionInstance expr)
                    {
                        byte[] copied = buf.ToArray();
                        Dictionary<string, object> lambdaReplacements = new();
                        context = context with { LambdaReplacements = lambdaReplacements };
                        for (int i = 0; i < copied.Length; i++)
                        {
                            lambdaReplacements["i"] = i;
                            lambdaReplacements["v"] = copied[i];
                            copied[i] |= CastUtil.CastByte(expr.Evaluate(context, ReadOnlySpan<byte>.Empty));
                        }
                        return new ReadOnlyMemory<byte>(copied);
                    }
                    else
                    {
                        byte key = CastUtil.CastByte(args[1]);
                        byte[] copied = buf.ToArray();
                        for (int i = 0; i < copied.Length; i++)
                            copied[i] |= key;
                        return new ReadOnlyMemory<byte>(copied);
                    }
                }
                throw new ArgumentException("Expected buffer at pos 0");
            default:
                throw new ArgumentException("Invalid method signature");
        }
    }

    private static object Xor(StructureEvaluationContext context, params object?[] args)
    {
        switch (args.Length)
        {
            case 2:
                // <buf> <lambda|key>
                if (TryGetReadOnlyMemoryFromPossibleBuffer(args[0], out var buf))
                {
                    if (args[1] is ExpressionInstance expr)
                    {
                        byte[] copied = buf.ToArray();
                        Dictionary<string, object> lambdaReplacements = new();
                        context = context with { LambdaReplacements = lambdaReplacements };
                        for (int i = 0; i < copied.Length; i++)
                        {
                            lambdaReplacements["i"] = i;
                            lambdaReplacements["v"] = copied[i];
                            copied[i] ^= CastUtil.CastByte(expr.Evaluate(context, ReadOnlySpan<byte>.Empty));
                        }
                        return new ReadOnlyMemory<byte>(copied);
                    }
                    else
                    {
                        byte key = CastUtil.CastByte(args[1]);
                        byte[] copied = buf.ToArray();
                        for (int i = 0; i < copied.Length; i++)
                            copied[i] ^= key;
                        return new ReadOnlyMemory<byte>(copied);
                    }
                }
                throw new ArgumentException("Expected buffer at pos 0");
            default:
                throw new ArgumentException("Invalid method signature");
        }
    }

    /// <summary>
    /// Create default method dictionary with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, MethodCallDelegate> CreateDefaultMethodDictionary()
    {
        return new Dictionary<string, MethodCallDelegate>(s_defaultMethods);
    }

    private static readonly Dictionary<string, DeserializerDefinition> s_defaultDeserializers = new()
    {
        { "buf", new BufferDeserializerDefinition() },
        { "byte", new PrimitiveDeserializerDefinition(typeof(byte), true) },
        { "sbyte", new PrimitiveDeserializerDefinition(typeof(sbyte), true) },
        { "ushort", new PrimitiveDeserializerDefinition(typeof(ushort), true) },
        { "short", new PrimitiveDeserializerDefinition(typeof(short), true) },
        { "uint", new PrimitiveDeserializerDefinition(typeof(uint), true) },
        { "int", new PrimitiveDeserializerDefinition(typeof(int), true) },
        { "ulong", new PrimitiveDeserializerDefinition(typeof(ulong), true) },
        { "long", new PrimitiveDeserializerDefinition(typeof(long), true) },
        { "byteb", new PrimitiveDeserializerDefinition(typeof(byte), false) },
        { "sbyteb", new PrimitiveDeserializerDefinition(typeof(sbyte), false) },
        { "ushortb", new PrimitiveDeserializerDefinition(typeof(ushort), false) },
        { "shortb", new PrimitiveDeserializerDefinition(typeof(short), false) },
        { "uintb", new PrimitiveDeserializerDefinition(typeof(uint), false) },
        { "intb", new PrimitiveDeserializerDefinition(typeof(int), false) },
        { "ulongb", new PrimitiveDeserializerDefinition(typeof(ulong), false) },
        { "longb", new PrimitiveDeserializerDefinition(typeof(long), false) },
        { "float", new PrimitiveDeserializerDefinition(typeof(float), false) },
        { "double", new PrimitiveDeserializerDefinition(typeof(double), false) },
        { "string", new StringDeserializerDefinition(StringDeserializerMode.Utf8Fixed) },
        { "cstring", new StringDeserializerDefinition(StringDeserializerMode.Utf8Null) },
        { "string16", new StringDeserializerDefinition(StringDeserializerMode.Utf16Fixed) },
        { "cstring16", new StringDeserializerDefinition(StringDeserializerMode.Utf16Null) },
    };

    /// <summary>
    /// Create default deserializer registry with standard deserializers
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, DeserializerDefinition> CreateDefaultDeserializerRegistry()
    {
        return new Dictionary<string, DeserializerDefinition>(s_defaultDeserializers);
    }

    private static readonly Dictionary<string, IExporter> s_defaultExporters = new() { { DataExporter.ExporterName, new DataExporter() }, { DecompressExporter.ExporterName, new DecompressExporter() } };

    /// <summary>
    /// Create default exporter dictionary with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, IExporter> CreateDefaultExporterDictionary()
    {
        return new Dictionary<string, IExporter>(s_defaultExporters);
    }

    internal static bool TryGetReadOnlyMemoryFromPossibleBuffer(object? source, out ReadOnlyMemory<byte> buffer)
    {
        switch (source)
        {
            case Memory<byte> memory:
                buffer = memory;
                return true;
            case ReadOnlyMemory<byte> readOnlyMemory:
                buffer = readOnlyMemory;
                return true;
            case byte[] array:
                buffer = array;
                return true;
            case MemoryStream memoryStream:
                if (memoryStream.TryGetBuffer(out ArraySegment<byte> segment))
                {
                    buffer = segment;
                    return true;
                }
                break;
        }
        buffer = ReadOnlyMemory<byte>.Empty;
        return false;
    }

    internal static void TrimStart(ref ReadOnlyMemory<byte> memory, StructureInstance instance, long offset)
    {
        int absoluteOffset;
        checked
        {
            absoluteOffset = (int)(instance.AbsoluteOffset + offset);
        }
        if (absoluteOffset < 0)
        {
            throw new ArgumentException("Start cannot be negative");
        }
        if (absoluteOffset > memory.Length)
        {
            throw new ArgumentException("Region exceeds buffer");
        }
        memory = memory.Slice(absoluteOffset);
    }

    internal static void TrimStart(ref ReadOnlySpan<byte> span, StructureInstance instance, long offset)
    {
        int absoluteOffset;
        checked
        {
            absoluteOffset = (int)(instance.AbsoluteOffset + offset);
        }
        if (absoluteOffset < 0)
        {
            throw new ArgumentException("Start cannot be negative");
        }
        if (absoluteOffset > span.Length)
        {
            throw new ArgumentException("Region exceeds buffer");
        }
        span = span.Slice(absoluteOffset);
    }

    internal static void TrimRange(Stream stream, StructureInstance instance, LongRange range)
    {
        long absoluteOffset = instance.AbsoluteOffset + range.Offset, length = range.Length;
        if (absoluteOffset < 0)
        {
            throw new ArgumentException("Start cannot be negative");
        }
        if (length < 0)
        {
            throw new ArgumentException("Length cannot be negative");
        }
        if (absoluteOffset + length > stream.Length)
        {
            throw new ArgumentException("Region exceeds buffer");
        }
        stream.Position = absoluteOffset;
    }

    internal static void TrimRange(ref ReadOnlyMemory<byte> memory, StructureInstance instance, LongRange range)
    {
        int absoluteOffset, length;
        checked
        {
            absoluteOffset = (int)(instance.AbsoluteOffset + range.Offset);
            length = (int)range.Length;
        }
        if (absoluteOffset < 0)
        {
            throw new ArgumentException("Start cannot be negative");
        }
        if (length < 0)
        {
            throw new ArgumentException("Length cannot be negative");
        }
        if (absoluteOffset + length > memory.Length)
        {
            throw new ArgumentException("Region exceeds buffer");
        }
        memory = memory.Slice(absoluteOffset, length);
    }

    internal static void TrimRange(ref ReadOnlySpan<byte> span, StructureInstance instance, LongRange range)
    {
        int absoluteOffset, length;
        checked
        {
            absoluteOffset = (int)(instance.AbsoluteOffset + range.Offset);
            length = (int)range.Length;
        }
        if (absoluteOffset < 0)
        {
            throw new ArgumentException("Start cannot be negative");
        }
        if (length < 0)
        {
            throw new ArgumentException("Length cannot be negative");
        }
        if (absoluteOffset + length > span.Length)
        {
            throw new ArgumentException("Region exceeds buffer");
        }
        span = span.Slice(absoluteOffset, length);
    }
}
