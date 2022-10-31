using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fp;
using Linear.Utility;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Deserializer that retrieves memory buffer.
/// </summary>
public class BufferDeserializerDefinition : DeserializerDefinition
{
    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return Enumerable.Empty<Element>();
    }

    /// <inheritdoc />
    public override IDeserializer GetInstance() => new BufferDeserializer();
}

/// <summary>
/// Deserializer that retrieves memory buffer.
/// </summary>
public class BufferDeserializer : IDeserializer
{
    /// <inheritdoc />
    public string? GetTargetTypeName() => null;

    /// <inheritdoc />
    public Type GetTargetType() => typeof(ReadOnlyMemory<byte>);

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, Stream stream, long offset, long? length = null, int index = 0)
    {
        if (length == null) throw new ArgumentException("Length required for buffer deserializer");
        LinearUtil.TrimRange(stream, context.Structure, new LongRange(offset, length.Value));
        byte[] result = new byte[length.Value];
        Processor.Read(stream, result, false);
        return new DeserializeResult(new ReadOnlyMemory<byte>(result), length.Value);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory, long offset, long? length = null, int index = 0)
    {
        if (length == null) throw new ArgumentException("Length required for buffer deserializer");
        LinearUtil.TrimRange(ref memory, context.Structure, new LongRange(offset, length.Value));
        return new DeserializeResult(memory, memory.Length);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span, long offset, long? length = null, int index = 0)
    {
        if (length == null) throw new ArgumentException("Length required for buffer deserializer");
        LinearUtil.TrimRange(ref span, context.Structure, new LongRange(offset, length.Value));
        return new DeserializeResult(new ReadOnlyMemory<byte>(span.ToArray()), span.Length);
    }
}
