using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Deserializes structure.
/// </summary>
public class StructureDeserializerDefinition : DeserializerDefinition
{
    private readonly string _name;

    /// <summary>
    /// Initializes an instance of <see cref="StructureDeserializerDefinition"/>.
    /// </summary>
    /// <param name="name">Name of structure.</param>
    public StructureDeserializerDefinition(string name)
    {
        _name = name;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return Enumerable.Empty<Element>();
    }

    /// <inheritdoc />
    public override IDeserializer GetInstance() => new StructureDeserializer(_name);
}

/// <summary>
/// Deserializes structure.
/// </summary>
public class StructureDeserializer : IDeserializer
{
    private readonly string _name;

    /// <summary>
    /// Initializes an instance of <see cref="StructureDeserializer"/>.
    /// </summary>
    /// <param name="name">Name of structure.</param>
    public StructureDeserializer(string name)
    {
        _name = name;
    }

    /// <inheritdoc />
    public string GetTargetTypeName() => _name;

    /// <inheritdoc />
    public Type GetTargetType() => typeof(StructureInstance);

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, Stream stream, long offset, long? length = null, int index = 0)
    {
        StructureInstance i = context.Structure.Registry[_name].Parse(context.Structure.Registry, stream, new ParseState(_name, offset, context.Structure, length, index));
        return new DeserializeResult(i, i.Length);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory, long offset, long? length = null, int index = 0)
    {
        StructureInstance i = context.Structure.Registry[_name].Parse(context.Structure.Registry, memory, new ParseState(_name, offset, context.Structure, length, index));
        return new DeserializeResult(i, i.Length);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span, long offset, long? length = null, int index = 0)
    {
        StructureInstance i = context.Structure.Registry[_name].Parse(context.Structure.Registry, span, new ParseState(_name, offset, context.Structure, length, index));
        return new DeserializeResult(i, i.Length);
    }
}
