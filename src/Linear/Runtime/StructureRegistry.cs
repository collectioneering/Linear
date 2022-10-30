using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Structures.
    /// </summary>
    public IReadOnlyDictionary<string, Structure> Structures => _structures;

    /// <summary>
    /// Deserializers.
    /// </summary>
    public IReadOnlyDictionary<string, IDeserializer> Deserializers => _deserializers;

    /// <summary>
    /// Methods.
    /// </summary>
    public IReadOnlyDictionary<string, MethodCallDelegate> Methods => _methods;

    private readonly Dictionary<string, Structure> _structures;
    private readonly Dictionary<string, IDeserializer> _deserializers;
    private readonly Dictionary<string, MethodCallDelegate> _methods;

    /// <summary>
    /// Initializes an instance of <see cref="StructureRegistry"/>.
    /// </summary>
    public StructureRegistry()
    {
        _structures = new Dictionary<string, Structure>();
        _deserializers = LinearUtil.CreateDefaultDeserializerRegistry();
        _methods = LinearUtil.CreateDefaultMethodDictionary();
    }

    /// <summary>
    /// Adds a structure to this registry.
    /// </summary>
    /// <param name="name">Target name.</param>
    /// <param name="structure">Structure to add.</param>
    public void AddStructure(string name, Structure structure)
    {
        if (_structures.ContainsKey(name))
        {
            throw new InvalidOperationException($"Cannot add a structure with the name \"{name}\" - one already exists");
        }
        if (Deserializers.ContainsKey(name))
        {
            throw new InvalidOperationException($"Cannot add a structure with the name \"{name}\" - a deserializer using that name already exists");
        }
        _structures.Add(name, structure);
        _deserializers.Add(name, new StructureDeserializer(name));
    }

    /// <summary>
    /// Adds a deserializer to this registry.
    /// </summary>
    /// <param name="name">Target name.</param>
    /// <param name="deserializer">Deserializer to add.</param>
    public void AddDeserializer(string name, IDeserializer deserializer)
    {
        if (Deserializers.ContainsKey(name))
        {
            throw new InvalidOperationException($"Cannot add a deserializer with the name \"{name}\" - one already exists");
        }
        _deserializers.Add(name, deserializer);
    }

    /// <summary>
    /// Adds a method to this registry.
    /// </summary>
    /// <param name="name">Target name.</param>
    /// <param name="method">Method to add.</param>
    public void AddMethod(string name, MethodCallDelegate method)
    {
        if (Methods.ContainsKey(name))
        {
            throw new InvalidOperationException($"Cannot add a method with the name \"{name}\" - one already exists");
        }
        _methods.Add(name, method);
    }

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
    /// <param name="input">Lyn format input.</param>
    /// <param name="filenameHint">Filename hint.</param>
    /// <param name="errorHandler">Parser error handler.</param>
    /// <returns>True if succeeded.</returns>
    public void Load(string input, string? filenameHint = null, IAntlrErrorStrategy? errorHandler = null)
    {
        Load(new StringReader(input), filenameHint, errorHandler);
    }

    /// <summary>
    /// Parses and adds structures.
    /// </summary>
    /// <param name="input">Lyn format input.</param>
    /// <param name="logDelegate">Log delegate.</param>
    /// <param name="filenameHint">Filename hint.</param>
    /// <param name="errorHandler">Parser error handler.</param>
    /// <returns>True if succeeded.</returns>
    public bool TryLoad(string input, Action<string> logDelegate, string? filenameHint = null, IAntlrErrorStrategy? errorHandler = null)
    {
        return TryLoad(new StringReader(input), logDelegate, filenameHint, errorHandler);
    }

    /// <summary>
    /// Parses and adds structures.
    /// </summary>
    /// <param name="input">Lyn format reader.</param>
    /// <param name="filenameHint">Filename hint.</param>
    /// <param name="errorHandler">Parser error handler.</param>
    /// <returns>True if succeeded.</returns>
    public void Load(TextReader input, string? filenameHint = null, IAntlrErrorStrategy? errorHandler = null)
    {
        var inputStream = new AntlrInputStream(input);
        var lexer = new LinearLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new LinearParser(tokens);
        Load(parser, filenameHint, errorHandler, Deserializers, Methods, out var createdDeserializers, out var structures);
        Add(createdDeserializers, structures);
    }

    /// <summary>
    /// Parses and adds structures.
    /// </summary>
    /// <param name="input">Lyn format reader.</param>
    /// <param name="logDelegate">Log delegate.</param>
    /// <param name="filenameHint">Filename hint.</param>
    /// <param name="errorHandler">Parser error handler.</param>
    /// <returns>True if succeeded.</returns>
    public bool TryLoad(TextReader input, Action<string> logDelegate, string? filenameHint = null, IAntlrErrorStrategy? errorHandler = null)
    {
        var inputStream = new AntlrInputStream(input);
        var lexer = new LinearLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new LinearParser(tokens);
        try
        {
            Load(parser, filenameHint, errorHandler, Deserializers, Methods, out var createdDeserializers, out var structures);
            Add(createdDeserializers, structures);
        }
        catch (InvalidOperationException e)
        {
            logDelegate(e.Message);
            return false;
        }
        catch (LynFormatException e)
        {
            foreach (string? error in e.Errors)
            {
                logDelegate(error);
            }
            logDelegate(e.Message);
            return false;
        }
        return true;
    }

    private void Add(List<KeyValuePair<string, IDeserializer>> createdDeserializers, List<KeyValuePair<string, Structure>> structures)
    {
        List<string> existingDeserializers = Deserializers.Keys.Intersect(createdDeserializers.Select(v => v.Key)).ToList();
        if (existingDeserializers.Count != 0)
        {
            StringBuilder messageBuilder = new("Existing deserializers ");
            messageBuilder.AppendJoin(", ", existingDeserializers);
            messageBuilder.Append("with the same name cannot be replaced");
            throw new InvalidOperationException(messageBuilder.ToString());
        }
        List<string> existingStructures = _structures.Keys.Intersect(structures.Select(v => v.Key)).ToList();
        if (existingStructures.Count != 0)
        {
            StringBuilder messageBuilder = new("Existing structures ");
            messageBuilder.AppendJoin(", ", existingStructures);
            messageBuilder.Append("with the same name cannot be replaced");
            throw new InvalidOperationException(messageBuilder.ToString());
        }
        foreach (var structure in structures)
        {
            _structures.Add(structure.Key, structure.Value);
        }
        foreach (var pair in createdDeserializers)
        {
            _deserializers.Add(pair.Key, pair.Value);
        }
    }

    private static void Load(LinearParser parser, string? filenameHint, IAntlrErrorStrategy? errorHandler,
        IReadOnlyDictionary<string, IDeserializer> deserializers, IReadOnlyDictionary<string, MethodCallDelegate> methods,
        out List<KeyValuePair<string, IDeserializer>> createdDeserializers,
        out List<KeyValuePair<string, Structure>> structures)
    {
        var listenerPre = new LinearPreListener();
        if (errorHandler != null)
            parser.ErrorHandler = errorHandler;
        ParseTreeWalker.Default.Walk(listenerPre, parser.compilation_unit());
        if (listenerPre.Fail)
        {
            throw new InvalidOperationException("Failed to parse structure");
        }
        createdDeserializers = listenerPre.GetStructureNames().Select(v => new KeyValuePair<string, IDeserializer>(v, new StructureDeserializer(v))).ToList();
        Dictionary<string, IDeserializer> deserializersTmp = new(deserializers.Concat(createdDeserializers));
        var listener = new LinearListener(deserializersTmp, methods, filenameHint);
        parser.Reset();
        ParseTreeWalker.Default.Walk(listener, parser.compilation_unit());
        if (listener.Fail)
        {
            throw new LynFormatException(listener.GetErrors());
        }
        structures = new List<KeyValuePair<string, Structure>>();
        foreach (StructureDefinition structure in listener.GetStructures())
        {
            var built = structure.Build();
            structures.Add(new KeyValuePair<string, Structure>(structure.Name, built));
        }
    }
}

internal class LynFormatException : IOException
{
    public IReadOnlyList<string> Errors { get; }

    public LynFormatException(IReadOnlyList<string> errors) : base("Errors occurred while parsing format")
    {
        Errors = errors;
    }

    public LynFormatException(string message, IReadOnlyList<string> errors) : base(message)
    {
        Errors = errors;
    }
}
