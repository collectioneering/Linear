using System;
using System.IO;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Deserializes raw array
/// </summary>
public class ArrayDeserializer : IDeserializer
{
    private readonly IDeserializer _elementDeserializer;
    private readonly Type _elementType;
    private readonly Type _type;

    /// <summary>
    /// Create new instance of <see cref="ArrayDeserializer"/>
    /// </summary>
    /// <param name="elementDeserializer">Element deserializer</param>
    public ArrayDeserializer(IDeserializer elementDeserializer)
    {
        _elementDeserializer = elementDeserializer;
        _elementType = _elementDeserializer.GetTargetType();
        _type = _elementType.MakeArrayType();
    }

    /// <inheritdoc />
    public string GetTargetTypeName() => throw new NotSupportedException();

    /// <inheritdoc />
    public Type GetTargetType() => _type;

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, Stream stream,
        long offset, bool littleEndian, long? length = null, int index = 0)
    {
        int arrayLength;
        checked
        {
            arrayLength = (int)(context.ArrayLength ?? throw new InvalidOperationException("No array length specified"));
        }
        Array res = Array.CreateInstance(_elementType, arrayLength);
        long curOffset = offset;
        var elementContext = context with { Parameters = null };
        for (int i = 0; i < arrayLength; i++)
        {
            (object value, long? elemLength) = _elementDeserializer.Deserialize(elementContext, stream, curOffset, littleEndian, 0, i);
            res.SetValue(value, i);
            if (elemLength is { } elemLengthValue)
            {
                curOffset += elemLengthValue;
            }
            else
            {
                throw new InvalidOperationException("Unknown length for deserialized element");
            }
        }

        return new DeserializeResult(res, curOffset - offset);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory,
        long offset, bool littleEndian, long? length = null, int index = 0)
    {
        int arrayLength;
        checked
        {
            arrayLength = (int)(context.ArrayLength ?? throw new InvalidOperationException("No array length specified"));
        }
        Array res = Array.CreateInstance(_elementType, arrayLength);
        long curOffset = offset;
        var elementContext = context with { Parameters = null };
        for (int i = 0; i < arrayLength; i++)
        {
            (object value, long? elemLength) = _elementDeserializer.Deserialize(elementContext, memory, curOffset, littleEndian, 0, i);
            res.SetValue(value, i);
            if (elemLength is { } elemLengthValue)
            {
                curOffset += elemLengthValue;
            }
            else
            {
                throw new InvalidOperationException("Unknown length for deserialized element");
            }
        }

        return new DeserializeResult(res, curOffset - offset);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span,
        long offset, bool littleEndian, long? length = null, int index = 0)
    {
        int arrayLength;
        checked
        {
            arrayLength = (int)(context.ArrayLength ?? throw new InvalidOperationException("No array length specified"));
        }
        Array res = Array.CreateInstance(_elementType, arrayLength);
        long curOffset = offset;
        var elementContext = context with { Parameters = null };
        for (int i = 0; i < arrayLength; i++)
        {
            (object value, long? elemLength) = _elementDeserializer.Deserialize(elementContext, span, curOffset, littleEndian, 0, i);
            res.SetValue(value, i);
            if (elemLength is { } elemLengthValue)
            {
                curOffset += elemLengthValue;
            }
            else
            {
                throw new InvalidOperationException("Unknown length for deserialized element");
            }
        }

        return new DeserializeResult(res, curOffset - offset);
    }
}
