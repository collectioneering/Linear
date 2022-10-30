using System;
using System.Collections.Generic;
using System.Globalization;
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
    private static readonly Dictionary<string, MethodCallDelegate> s_defaultMethods = new() { { "log", Log }, { "format", Format } };

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

    /// <summary>
    /// Create default method dictionary with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, MethodCallDelegate> CreateDefaultMethodDictionary()
    {
        return new Dictionary<string, MethodCallDelegate>(s_defaultMethods);
    }

    private static readonly Dictionary<string, IDeserializer> s_defaultDeserializers = new()
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
    /// Create default deserializer registry with standard deserializers
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, IDeserializer> CreateDefaultDeserializerRegistry()
    {
        return new Dictionary<string, IDeserializer>(s_defaultDeserializers);
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

    internal static bool TryGetReadOnlySpanFromPossibleBuffer(object? source, out ReadOnlySpan<byte> buffer)
    {
        switch (source)
        {
            case Memory<byte> memory:
                buffer = memory.Span;
                return true;
            case ReadOnlyMemory<byte> readOnlyMemory:
                buffer = readOnlyMemory.Span;
                return true;
            case byte[] array:
                buffer = array;
                return true;
        }
        buffer = ReadOnlySpan<byte>.Empty;
        return false;
    }

    internal static void TrimExportTarget(ref ReadOnlyMemory<byte> memory, StructureInstance instance, LongRange range)
    {
        int absoluteOffset, length;
        checked
        {
            absoluteOffset = (int)(instance.AbsoluteOffset + range.Offset);
            length = (int)range.Length;
        }
        if (absoluteOffset < 0)
        {
            throw new ArgumentException("Export target start cannot be negative");
        }
        if (length < 0)
        {
            throw new ArgumentException("Export target length cannot be negative");
        }
        if (absoluteOffset + length > memory.Length)
        {
            throw new ArgumentException("Export target region exceeds buffer");
        }
        memory = memory.Slice(absoluteOffset, length);
    }

    internal static void TrimExportTarget(ref ReadOnlySpan<byte> span, StructureInstance instance, LongRange range)
    {
        int absoluteOffset, length;
        checked
        {
            absoluteOffset = (int)(instance.AbsoluteOffset + range.Offset);
            length = (int)range.Length;
        }
        if (absoluteOffset < 0)
        {
            throw new ArgumentException("Export target start cannot be negative");
        }
        if (length < 0)
        {
            throw new ArgumentException("Export target length cannot be negative");
        }
        if (absoluteOffset + length > span.Length)
        {
            throw new ArgumentException("Export target region exceeds buffer");
        }
        span = span.Slice(absoluteOffset, length);
    }
}
