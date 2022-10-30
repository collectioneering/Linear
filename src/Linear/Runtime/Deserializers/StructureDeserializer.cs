using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Deserializes structure
/// </summary>
public class StructureDeserializer : IDeserializer
{
    private readonly string _name;

    /// <summary>
    /// Create new instance of <see cref="StructureDeserializer"/>
    /// </summary>
    /// <param name="name">Name of structure</param>
    public StructureDeserializer(string name)
    {
        _name = name;
    }

    /// <inheritdoc />
    public string GetTargetTypeName() => _name;

    /// <inheritdoc />
    public Type GetTargetType() => typeof(StructureInstance);

    /// <inheritdoc />
    public DeserializeResult Deserialize(StructureInstance instance, Stream stream,
        long offset, bool littleEndian, Dictionary<StandardProperty, object>? standardProperties,
        Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        StructureInstance i = instance.Registry[_name].Parse(instance.Registry, stream, new ParseState(_name, offset, instance, length, index));
        return new DeserializeResult(i, i.Length);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(StructureInstance instance, ReadOnlyMemory<byte> memory,
        long offset, bool littleEndian, Dictionary<StandardProperty, object>? standardProperties,
        Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        return Deserialize(instance, memory.Span, offset, littleEndian, standardProperties, parameters, length, index);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(StructureInstance instance, ReadOnlySpan<byte> span,
        long offset, bool littleEndian, Dictionary<StandardProperty, object>? standardProperties,
        Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        StructureInstance i = instance.Registry[_name].Parse(instance.Registry, span, new ParseState(_name, offset, instance, length, index));
        return new DeserializeResult(i, i.Length);
    }
}
