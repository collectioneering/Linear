using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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
    /// <summary>
    /// Deserializers.
    /// </summary>
    public readonly Dictionary<string, IDeserializer> Deserializers;
    /// <summary>
    /// Methods.
    /// </summary>
    public readonly Dictionary<string, MethodCallDelegate> Methods;

    private readonly Dictionary<string, Structure> _structures;

    /// <summary>
    /// Initializes an instance of <see cref="StructureRegistry"/>.
    /// </summary>
    /// <param name="deserializers">Deserializers.</param>
    /// <param name="methods">Methods.</param>
    public StructureRegistry(IReadOnlyCollection<IDeserializer>? deserializers = null,
        IReadOnlyCollection<MethodCallExpression.NamedDelegate>? methods = null)
    {
        _structures = new Dictionary<string, Structure>();

        Deserializers = LinearUtil.CreateDefaultDeserializerRegistry();
        if (deserializers != null)
        {
            foreach (var deserializer in deserializers)
            {
                string? dname = deserializer.GetTargetTypeName();
                if (dname == null)
                {
                    throw new ArgumentException("Target name is required for all deserializers");
                }
                Deserializers[dname] = deserializer;
            }
        }
        Methods = LinearUtil.CreateDefaultMethodDictionary();
        if (methods != null)
        {
            foreach (var method in methods)
            {
                Methods[method.Name] = method.Delegate;
            }
        }
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
    public bool TryGetValue(string name, [NotNullWhen(true)] out Structure? structure) => _structures.TryGetValue(name, out structure);

    /// <summary>
    /// Parses and adds structures.
    /// </summary>
    /// <param name="input">Lyn format reader.</param>
    /// <param name="logDelegate">Log delegate.</param>
    /// <param name="errorHandler">Parser error handler.</param>
    /// <returns>True if succeeded.</returns>
    public bool TryLoad(TextReader input, Action<string> logDelegate, IAntlrErrorStrategy? errorHandler = null)
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
            return false;
        }

        var pairs = listenerPre.GetStructureNames().Select(v => new KeyValuePair<string, IDeserializer>(v, new StructureDeserializer(v))).ToList();
        Dictionary<string, IDeserializer> deserializersTmp = new(Deserializers.Concat(pairs));
        var listener = new LinearListener(deserializersTmp, Methods, logDelegate);
        parser.Reset();
        ParseTreeWalker.Default.Walk(listener, parser.compilation_unit());
        foreach (StructureDefinition structure in listener.GetStructures())
        {
            Add(structure.Build());
        }
        foreach (var pair in pairs)
        {
            Deserializers[pair.Key] = pair.Value;
        }
        return true;
    }
}
