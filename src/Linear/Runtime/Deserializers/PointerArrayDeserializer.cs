using System;
using System.Collections.Generic;
using System.IO;
using static Linear.Utility.CastUtil;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Deserializes pointer array
/// </summary>
public class PointerArrayDeserializer : IDeserializer
{
    private readonly IDeserializer _mainDeserializer;
    private readonly IDeserializer _elementDeserializer;
    private readonly Type _elementType;
    private readonly Type _type;
    private readonly bool _lenFinder;

    /// <summary>
    /// Create new instance of <see cref="ArrayDeserializer"/>
    /// </summary>
    /// <param name="mainDeserializer">Main array deserializer</param>
    /// <param name="elementDeserializer">Element deserializer</param>
    /// <param name="lenFinder">If true, uses space to determine element length (and requires n+1 elements)</param>
    public PointerArrayDeserializer(IDeserializer mainDeserializer, IDeserializer elementDeserializer, bool lenFinder)
    {
        _mainDeserializer = mainDeserializer;
        _elementDeserializer = elementDeserializer;
        _elementType = _elementDeserializer.GetTargetType();
        _type = _elementType.MakeArrayType();
        _lenFinder = lenFinder;
    }

    /// <inheritdoc />
    public string GetTargetTypeName() => throw new NotSupportedException();

    /// <inheritdoc />
    public Type GetTargetType() => _type;

    // TODO support using SourceWithOffset for index array source and SourceWithOffset for target region

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, Stream stream,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        (object src, _) = _mainDeserializer.Deserialize(context, stream, offset, littleEndian, parameters);
        Array baseArray = (Array)src;
        int pointerArrayLength;
        checked
        {
            pointerArrayLength = (int)(context.PointerArrayLength ?? throw new InvalidOperationException("No pointer array length specified"));
        }
        long pointerOffset = context.PointerOffset ?? throw new InvalidOperationException("No pointer offset specified");
        Array tarArray = Array.CreateInstance(_elementType, pointerArrayLength);
        long? curOffset = offset;
        for (int i = 0; i < pointerArrayLength; i++)
        {
            long vI = CastLong(baseArray.GetValue(i));
            long preElemLength = _lenFinder ? CastLong(baseArray.GetValue(i + 1)) - vI : 0;
            (object value, long? elemLength) = _elementDeserializer.Deserialize(context, stream, pointerOffset + vI, littleEndian, parameters, preElemLength, i);
            tarArray.SetValue(value, i);
            if (curOffset is { } curOffsetValue)
            {
                if (elemLength is { } elemLengthValue)
                {
                    curOffset = curOffsetValue + elemLengthValue;
                }
                else
                {
                    curOffset = null;
                }
            }
        }

        return new DeserializeResult(tarArray, curOffset.HasValue ? curOffset.Value - offset : null);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        (object src, _) = _mainDeserializer.Deserialize(context, memory, offset, littleEndian, parameters);
        Array baseArray = (Array)src;
        int pointerArrayLength;
        checked
        {
            pointerArrayLength = (int)(context.PointerArrayLength ?? throw new InvalidOperationException("No pointer array length specified"));
        }
        long pointerOffset = context.PointerOffset ?? throw new InvalidOperationException("No pointer offset specified");
        Array tarArray = Array.CreateInstance(_elementType, pointerArrayLength);
        long? curOffset = offset;
        for (int i = 0; i < pointerArrayLength; i++)
        {
            long vI = CastLong(baseArray.GetValue(i));
            long preElemLength = _lenFinder ? CastLong(baseArray.GetValue(i + 1)) - vI : 0;
            (object value, long? elemLength) = _elementDeserializer.Deserialize(context, memory, pointerOffset + vI, littleEndian, parameters, preElemLength, i);
            tarArray.SetValue(value, i);
            if (curOffset is { } curOffsetValue)
            {
                if (elemLength is { } elemLengthValue)
                {
                    curOffset = curOffsetValue + elemLengthValue;
                }
                else
                {
                    curOffset = null;
                }
            }
        }

        return new DeserializeResult(tarArray, curOffset.HasValue ? curOffset.Value - offset : null);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span,
        long offset, bool littleEndian, Dictionary<string, object>? parameters, long? length = null, int index = 0)
    {
        (object src, _) = _mainDeserializer.Deserialize(context, span, offset, littleEndian, parameters);
        Array baseArray = (Array)src;
        int pointerArrayLength;
        checked
        {
            pointerArrayLength = (int)(context.PointerArrayLength ?? throw new InvalidOperationException("No pointer array length specified"));
        }
        long pointerOffset = context.PointerOffset ?? throw new InvalidOperationException("No pointer offset specified");
        Array tarArray = Array.CreateInstance(_elementType, pointerArrayLength);
        long? curOffset = offset;
        for (int i = 0; i < pointerArrayLength; i++)
        {
            long vI = CastLong(baseArray.GetValue(i));
            long preElemLength = _lenFinder ? CastLong(baseArray.GetValue(i + 1)) - vI : 0;
            (object value, long? elemLength) = _elementDeserializer.Deserialize(context, span, pointerOffset + vI, littleEndian, parameters, preElemLength, i);
            tarArray.SetValue(value, i);
            if (curOffset is { } curOffsetValue)
            {
                if (elemLength is { } elemLengthValue)
                {
                    curOffset = curOffsetValue + elemLengthValue;
                }
                else
                {
                    curOffset = null;
                }
            }
        }

        return new DeserializeResult(tarArray, curOffset.HasValue ? curOffset.Value - offset : null);
    }
}
