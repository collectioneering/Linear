using System;
using System.Collections.Generic;
using System.IO;
using Fp;
using Linear.Utility;

namespace Linear.Runtime.Deserializers;

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
    public DeserializeResult Deserialize(StructureInstance instance, Stream stream, long offset, bool littleEndian,
        Dictionary<StandardProperty, object>? standardProperties, Dictionary<string, object>? parameters,
        long? length = null, int index = 0)
    {
        if (length == null) throw new ArgumentException("Length required for buffer deserializer");
        offset += instance.AbsoluteOffset;
        int lengthValue;
        checked
        {
            lengthValue = (int)length.Value;
        }
        stream.Position = offset;
        byte[] result = new byte[lengthValue];
        Processor.Read(stream, result, false);
        return new DeserializeResult(new ReadOnlyMemory<byte>(result), lengthValue);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(StructureInstance instance, ReadOnlyMemory<byte> memory,
        long offset, bool littleEndian, Dictionary<StandardProperty, object>? standardProperties,
        Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        if (length == null) throw new ArgumentException("Length required for buffer deserializer");
        LinearUtil.TrimRange(ref memory, instance, new LongRange(offset, length.Value));
        return new DeserializeResult(memory, memory.Length);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(StructureInstance instance, ReadOnlySpan<byte> span, long offset,
        bool littleEndian, Dictionary<StandardProperty, object>? standardProperties,
        Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        if (length == null) throw new ArgumentException("Length required for buffer deserializer");
        LinearUtil.TrimRange(ref span, instance, new LongRange(offset, length.Value));
        return new DeserializeResult(new ReadOnlyMemory<byte>(span.ToArray()), span.Length);
    }
}
