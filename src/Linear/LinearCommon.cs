using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        return string.Format(CultureInfo.InvariantCulture, args[0]?.ToString() ?? "", args.Skip(1).ToArray());
    }

    /// <summary>
    /// Create default method dictionary with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, MethodCallExpression.MethodCallDelegate> CreateDefaultMethodDictionary()
    {
        return new Dictionary<string, MethodCallExpression.MethodCallDelegate>(s_defaultMethods);
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
    /// Create default deserializer registry with standard exporters
    /// </summary>
    /// <returns>Default registry</returns>
    public static Dictionary<string, IDeserializer> CreateDefaultDeserializerRegistry()
    {
        return new Dictionary<string, IDeserializer>(s_defaultDeserializers);
    }
}
