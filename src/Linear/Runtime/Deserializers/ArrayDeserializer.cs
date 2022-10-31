using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linear.Runtime.Expressions;
using static Linear.Utility.CastUtil;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Deserializes raw array.
/// </summary>
public class ArrayDeserializerDefinition : DeserializerDefinition
{
    private readonly DeserializerDefinition _elementDeserializer;
    private readonly MemberExpression _countExpression;

    /// <summary>
    /// Initializes an instance of <see cref="ArrayDeserializer"/>.
    /// </summary>
    /// <param name="elementDeserializer">Element deserializer.</param>
    /// <param name="countExpression">Count expression.</param>
    public ArrayDeserializerDefinition(DeserializerDefinition elementDeserializer, MemberExpression countExpression)
    {
        _elementDeserializer = elementDeserializer;
        _countExpression = countExpression;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) => _elementDeserializer.GetDependencies(definition).Union(_countExpression.GetDependencies(definition));

    /// <inheritdoc />
    public override DeserializerInstance GetInstance() => new ArrayDeserializer(_elementDeserializer.GetInstance(), _countExpression.GetInstance());
}

/// <summary>
/// Deserializes raw array.
/// </summary>
public class ArrayDeserializer : DeserializerInstance
{
    private readonly DeserializerInstance _elementDeserializer;
    private readonly ExpressionInstance _countExpression;
    private readonly Type _elementType;
    private readonly Type _type;

    /// <summary>
    /// Initializes an instance of <see cref="ArrayDeserializer"/>.
    /// </summary>
    /// <param name="elementDeserializer">Element deserializer.</param>
    /// <param name="countExpression">Count expression.</param>
    public ArrayDeserializer(DeserializerInstance elementDeserializer, ExpressionInstance countExpression)
    {
        _elementDeserializer = elementDeserializer;
        _countExpression = countExpression;
        _elementType = _elementDeserializer.GetTargetType();
        _type = _elementType.MakeArrayType();
    }

    /// <inheritdoc />
    public override Type GetTargetType() => _type;

    /// <inheritdoc />
    public override DeserializeResult Deserialize(DeserializerContext context, Stream stream, long offset, long? length = null, int index = 0)
    {
        var structureContext = new StructureEvaluationContext(context.Structure);
        int arrayLength = CastInt(_countExpression.Evaluate(structureContext, ReadOnlySpan<byte>.Empty));
        Array res = Array.CreateInstance(_elementType, arrayLength);
        long curOffset = offset;
        var elementContext = context with { Parameters = null };
        for (int i = 0; i < arrayLength; i++)
        {
            (object value, long? elemLength) = _elementDeserializer.Deserialize(elementContext, stream, curOffset, 0, i);
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
    public override DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory, long offset, long? length = null, int index = 0)
    {
        var structureContext = new StructureEvaluationContext(context.Structure);
        int arrayLength = CastInt(_countExpression.Evaluate(structureContext, ReadOnlySpan<byte>.Empty));
        Array res = Array.CreateInstance(_elementType, arrayLength);
        long curOffset = offset;
        var elementContext = context with { Parameters = null };
        for (int i = 0; i < arrayLength; i++)
        {
            (object value, long? elemLength) = _elementDeserializer.Deserialize(elementContext, memory, curOffset, 0, i);
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
    public override DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span, long offset, long? length = null, int index = 0)
    {
        var structureContext = new StructureEvaluationContext(context.Structure);
        int arrayLength = CastInt(_countExpression.Evaluate(structureContext, ReadOnlySpan<byte>.Empty));
        Array res = Array.CreateInstance(_elementType, arrayLength);
        long curOffset = offset;
        var elementContext = context with { Parameters = null };
        for (int i = 0; i < arrayLength; i++)
        {
            (object value, long? elemLength) = _elementDeserializer.Deserialize(elementContext, span, curOffset, 0, i);
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
