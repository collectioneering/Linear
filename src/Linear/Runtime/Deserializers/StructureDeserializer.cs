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
    public DeserializeResult Deserialize(DeserializerContext context, Stream stream,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        StructureInstance i = context.Structure.Registry[_name].Parse(context.Structure.Registry, stream, new ParseState(_name, offset, context.Structure, length, index));
        return new DeserializeResult(i, i.Length);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        StructureInstance i = context.Structure.Registry[_name].Parse(context.Structure.Registry, memory, new ParseState(_name, offset, context.Structure, length, index));
        return new DeserializeResult(i, i.Length);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        StructureInstance i = context.Structure.Registry[_name].Parse(context.Structure.Registry, span, new ParseState(_name, offset, context.Structure, length, index));
        return new DeserializeResult(i, i.Length);
    }
}
