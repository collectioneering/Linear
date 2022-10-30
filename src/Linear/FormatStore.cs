using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Linear.Runtime;
using Linear.Runtime.Expressions;

namespace Linear;

/// <summary>
/// Represents a store of processed <see cref="Structure"/>.
/// </summary>
public class FormatStore : IEnumerable<string>
{
    /// <summary>
    /// Structures.
    /// </summary>
    public IReadOnlyDictionary<string, Structure> Structures => _registry.Structures;

    /// <summary>
    /// Deserializers.
    /// </summary>
    public IReadOnlyDictionary<string, IDeserializer> Deserializers => _registry.Deserializers;

    /// <summary>
    /// Methods.
    /// </summary>
    public IReadOnlyDictionary<string, MethodCallDelegate> Methods => _registry.Methods;

    internal readonly StructureRegistry _registry;

    private readonly List<string> _specs;

    /// <summary>
    /// Initializes an instance of <see cref="FormatStore"/>.
    /// </summary>
    public FormatStore()
    {
        _registry = new StructureRegistry();
        _specs = new List<string>();
    }

    /// <summary>
    /// Initializes an instance of <see cref="FormatStore"/>.
    /// </summary>
    /// <param name="deserializers">Deserializers.</param>
    /// <param name="methods">Methods.</param>
    public FormatStore(IEnumerable<KeyValuePair<string, IDeserializer>> deserializers, IEnumerable<KeyValuePair<string, MethodCallDelegate>> methods)
    {
        _registry = new StructureRegistry();
        foreach (var pair in deserializers)
            _registry.AddDeserializer(pair.Key, pair.Value);
        foreach (var pair in methods)
            _registry.AddMethod(pair.Key, pair.Value);
        _specs = new List<string>();
    }

    /// <summary>
    /// Adds a structure spec.
    /// </summary>
    /// <param name="linearLayoutSpec">Linear layout spec.</param>
    public void Add(string linearLayoutSpec)
    {
        _registry.Load(linearLayoutSpec);
        _specs.Add(linearLayoutSpec);
    }

    /// <summary>
    /// Adds a deserializer to this registry.
    /// </summary>
    /// <param name="name">Target name.</param>
    /// <param name="deserializer">Deserializer to add.</param>
    public void AddDeserializer(string name, IDeserializer deserializer)
    {
        _registry.AddDeserializer(name, deserializer);
    }

    /// <summary>
    /// Adds a method to this registry.
    /// </summary>
    /// <param name="name">Target name.</param>
    /// <param name="method">Method to add.</param>
    public void AddMethod(string name, MethodCallDelegate method)
    {
        _registry.AddMethod(name, method);
    }

    /// <summary>
    /// Attempts to get structure by name.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="structure">Structure.</param>
    /// <returns>True if found.</returns>
    public bool TryGetStructure(string name, [NotNullWhen(true)] out Structure? structure) => _registry.TryGetStructure(name, out structure);

    /// <summary>
    /// Parses structure from stream.
    /// </summary>
    /// <param name="name">Structure name.</param>
    /// <param name="stream">Stream to read from.</param>
    /// <returns>Parsed structure.</returns>
    public StructureInstance Parse(string name, Stream stream)
    {
        return _registry.Parse(name, stream);
    }

    /// <summary>
    /// Parses structure from stream.
    /// </summary>
    /// <param name="name">Structure name.</param>
    /// <param name="stream">Stream to read from.</param>
    /// <param name="parseState">Initial parse state.</param>
    /// <returns>Parsed structure.</returns>
    public StructureInstance Parse(string name, Stream stream, ParseState parseState)
    {
        return _registry.Parse(name, stream, parseState);
    }

    /// <inheritdoc />
    public IEnumerator<string> GetEnumerator() => _specs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
