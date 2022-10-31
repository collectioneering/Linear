using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linear.Runtime.Expressions;
using static Linear.Utility.CastUtil;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Deserializes pointer array.
/// </summary>
public class PointerArrayDeserializerDefinition : DeserializerDefinition
{
    private readonly MemberExpression _mainExpression;
    private readonly MemberExpression _countExpression;
    private readonly DeserializerDefinition _elementDeserializer;
    private readonly bool _lenFinder;

    /// <summary>
    /// Initializes an instance of <see cref="PointerArrayDeserializerDefinition"/>.
    /// </summary>
    /// <param name="mainExpression">Main array expression.</param>
    /// <param name="countExpression">Count expression.</param>
    /// <param name="elementDeserializer">Element deserializer.</param>
    /// <param name="lenFinder">If true, uses space to determine element length (and requires n+1 elements).</param>
    public PointerArrayDeserializerDefinition(MemberExpression mainExpression, MemberExpression countExpression, DeserializerDefinition elementDeserializer, bool lenFinder)
    {
        _mainExpression = mainExpression;
        _countExpression = countExpression;
        _elementDeserializer = elementDeserializer;
        _lenFinder = lenFinder;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return _mainExpression.GetDependencies(definition).Union(_countExpression.GetDependencies(definition)).Union(_elementDeserializer.GetDependencies(definition));
    }

    /// <inheritdoc />
    public override DeserializerInstance GetInstance() => new PointerArrayDeserializer(_mainExpression.GetInstance(), _countExpression.GetInstance(), _elementDeserializer.GetInstance(), _lenFinder);
}

/// <summary>
/// Deserializes pointer array.
/// </summary>
public class PointerArrayDeserializer : DeserializerInstance
{
    private readonly ExpressionInstance _mainExpression;
    private readonly ExpressionInstance _countExpression;
    private readonly DeserializerInstance _elementDeserializer;
    private readonly Type _elementType;
    private readonly Type _type;
    private readonly bool _lenFinder;

    /// <summary>
    /// Initializes an instance of <see cref="PointerArrayDeserializer"/>.
    /// </summary>
    /// <param name="mainExpression">Main array expression.</param>
    /// <param name="countExpression">Count expression.</param>
    /// <param name="elementDeserializer">Element deserializer.</param>
    /// <param name="lenFinder">If true, uses space to determine element length (and requires n+1 elements).</param>
    public PointerArrayDeserializer(ExpressionInstance mainExpression, ExpressionInstance countExpression, DeserializerInstance elementDeserializer, bool lenFinder)
    {
        _mainExpression = mainExpression;
        _countExpression = countExpression;
        _elementDeserializer = elementDeserializer;
        _elementType = elementDeserializer.GetTargetType();
        _type = _elementType.MakeArrayType();
        _lenFinder = lenFinder;
    }

    /// <inheritdoc />
    public override Type GetTargetType() => _type;

    /// <inheritdoc />
    public override DeserializeResult Deserialize(DeserializerContext context, Stream stream, long offset, long? length = null, int index = 0)
    {
        var structureContext = new StructureEvaluationContext(context.Structure);
        object src = _mainExpression.Evaluate(structureContext, stream) ?? throw new NullReferenceException();
        Array baseArray = (Array)src;
        int pointerArrayLength = CastInt(_countExpression.Evaluate(structureContext, ReadOnlySpan<byte>.Empty));
        Array tarArray = Array.CreateInstance(_elementType, pointerArrayLength);
        long? curOffset = offset;
        for (int i = 0; i < pointerArrayLength; i++)
        {
            long vI = CastLong(baseArray.GetValue(i));
            long preElemLength = _lenFinder ? CastLong(baseArray.GetValue(i + 1)) - vI : 0;
            (object value, long? elemLength) = _elementDeserializer.Deserialize(context, stream, offset + vI, preElemLength, i);
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
    public override DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory, long offset, long? length = null, int index = 0)
    {
        var structureContext = new StructureEvaluationContext(context.Structure);
        object src = _mainExpression.Evaluate(structureContext, memory) ?? throw new NullReferenceException();
        Array baseArray = (Array)src;
        int pointerArrayLength = CastInt(_countExpression.Evaluate(structureContext, ReadOnlySpan<byte>.Empty));
        Array tarArray = Array.CreateInstance(_elementType, pointerArrayLength);
        long? curOffset = offset;
        for (int i = 0; i < pointerArrayLength; i++)
        {
            long vI = CastLong(baseArray.GetValue(i));
            long preElemLength = _lenFinder ? CastLong(baseArray.GetValue(i + 1)) - vI : 0;
            (object value, long? elemLength) = _elementDeserializer.Deserialize(context, memory, offset + vI, preElemLength, i);
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
    public override DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span, long offset, long? length = null, int index = 0)
    {
        var structureContext = new StructureEvaluationContext(context.Structure);
        object src = _mainExpression.Evaluate(structureContext, span) ?? throw new NullReferenceException();
        Array baseArray = (Array)src;
        int pointerArrayLength = CastInt(_countExpression.Evaluate(structureContext, ReadOnlySpan<byte>.Empty));
        Array tarArray = Array.CreateInstance(_elementType, pointerArrayLength);
        long? curOffset = offset;
        for (int i = 0; i < pointerArrayLength; i++)
        {
            long vI = CastLong(baseArray.GetValue(i));
            long preElemLength = _lenFinder ? CastLong(baseArray.GetValue(i + 1)) - vI : 0;
            (object value, long? elemLength) = _elementDeserializer.Deserialize(context, span, offset + vI, preElemLength, i);
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
