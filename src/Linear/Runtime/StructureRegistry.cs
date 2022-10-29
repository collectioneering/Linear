using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Linear.Runtime.Deserializers;
using Linear.Runtime.Expressions;

namespace Linear.Runtime;

/// <summary>
/// Stores structures for cross-references
/// </summary>
public class StructureRegistry
{
    private readonly Dictionary<string, Structure> _structures;

    /// <summary>
    /// Create new instance of <see cref="StructureRegistry"/>
    /// </summary>
    public StructureRegistry()
    {
        _structures = new Dictionary<string, Structure>();
    }

    /// <summary>
    /// Add structure to registry
    /// </summary>
    /// <param name="structure">Structure</param>
    public void Add(Structure structure) => _structures.Add(structure.Name, structure);

    /// <summary>
    /// Get structure by name
    /// </summary>
    /// <param name="name">Name</param>
    /// <returns>Structure</returns>
    /// <exception cref="KeyNotFoundException">If structure not found</exception>
    public Structure this[string name] => _structures[name];

    /// <summary>
    /// Try to get structure by name
    /// </summary>
    /// <param name="name">Mae</param>
    /// <param name="structure">Structure</param>
    /// <returns>True if found</returns>
    public bool TryGetValue(string name, out Structure? structure) => _structures.TryGetValue(name, out structure);

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
    public static bool TryLoad(TextReader input, out StructureRegistry? registry, Action<string> logDelegate,
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

        Dictionary<string, IDeserializer> rDeserializers = LinearUtil.CreateDefaultDeserializerRegistry();
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

        Dictionary<string, MethodCallExpression.MethodCallDelegate> rMethods = LinearUtil.CreateDefaultMethodDictionary();
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
}
