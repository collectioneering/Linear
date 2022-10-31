﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linear.Utility;
using static Linear.Utility.CastUtil;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Deserializable expression
/// </summary>
public class DeserializeExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _offsetDefinition;
    private readonly IDeserializer _deserializer;
    private readonly DeserializerStandardProperties _standardProperties;
    private readonly Dictionary<string, ExpressionDefinition> _deserializerParams;

    /// <summary>
    /// Initializes an instance of <see cref="DeserializeExpression"/>.
    /// </summary>
    /// <param name="offsetDefinition">Offset value definition.</param>
    /// <param name="deserializer">Custom deserializer.</param>
    /// <param name="deserializerParams">Deserializer parameters.</param>
    /// <param name="standardProperties">Standard property expressions.</param>
    public DeserializeExpression(ExpressionDefinition offsetDefinition, IDeserializer deserializer,
        DeserializerStandardProperties standardProperties, Dictionary<string, ExpressionDefinition> deserializerParams)
    {
        _offsetDefinition = offsetDefinition;
        _deserializer = deserializer;
        _standardProperties = standardProperties;
        _deserializerParams = deserializerParams;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return _offsetDefinition.GetDependencies(definition)
            .Union(_deserializerParams.SelectMany(kvp => kvp.Value.GetDependencies(definition)))
            .Union(_standardProperties.GetDependencies(definition));
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() =>
        CreateDelegate(_offsetDefinition, _deserializer, _standardProperties, _deserializerParams);

    internal static ExpressionInstance CreateDelegate(
        ExpressionDefinition offsetDefinition, IDeserializer deserializer,
        DeserializerStandardProperties standardProperties,
        Dictionary<string, ExpressionDefinition> deserializerParams)
    {
        ExpressionInstance srcDelegate = offsetDefinition.GetInstance();
        Dictionary<string, ExpressionInstance> deserializerParamsCompact = new();
        foreach (var kvp in deserializerParams)
            deserializerParamsCompact[kvp.Key] = kvp.Value.GetInstance();
        return new DeserializeExpressionInstance(standardProperties.ToInstance(), deserializerParamsCompact, srcDelegate, deserializer);
    }

    private record DeserializeExpressionInstance(
        DeserializerStandardPropertiesInstance StandardPropertiesCompact,
        Dictionary<string, ExpressionInstance> DeserializerParamsCompact,
        ExpressionInstance Source, IDeserializer Deserializer) : ExpressionInstance
    {
        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, stream) ?? throw new NullReferenceException();
            var deserializerContext = new DeserializerContext(context.Structure, deserializerParamsGen);
            deserializerContext = StandardPropertiesCompact.Augment(deserializerContext, context, stream);
            object? source = Source.Evaluate(context, stream);
            if (source is SourceWithOffset swo)
            {
                return Extract(deserializerContext, swo);
            }
            LongRange range;
            if (TryCastLong(source, out long offsetValue))
                range = new LongRange(Offset: offsetValue, Length: 0);
            else if (source is LongRange r)
            {
                range = r;
            }
            else
            {
                throw new InvalidCastException("Cannot find offset or range type for source delegate");
            }
            return Deserializer.Deserialize(deserializerContext, stream, range.Offset, range.Length).Value;
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, memory) ?? throw new NullReferenceException();
            var deserializerContext = new DeserializerContext(context.Structure, deserializerParamsGen);
            deserializerContext = StandardPropertiesCompact.Augment(deserializerContext, context, memory);
            object? source = Source.Evaluate(context, memory);
            if (source is SourceWithOffset swo)
            {
                return Extract(deserializerContext, swo);
            }
            LongRange range;
            if (TryCastLong(source, out long offsetValue))
                range = new LongRange(Offset: offsetValue, Length: 0);
            else if (source is LongRange r)
            {
                range = r;
            }
            else
            {
                throw new InvalidCastException("Cannot find offset or range type for source delegate");
            }
            return Deserializer.Deserialize(deserializerContext, memory, range.Offset, range.Length).Value;
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, span) ?? throw new NullReferenceException();
            var deserializerContext = new DeserializerContext(context.Structure, deserializerParamsGen);
            deserializerContext = StandardPropertiesCompact.Augment(deserializerContext, context, span);
            object? source = Source.Evaluate(context, span);
            if (source is SourceWithOffset swo)
            {
                return Extract(deserializerContext, swo);
            }
            LongRange range;
            if (TryCastLong(source, out long offsetValue))
                range = new LongRange(Offset: offsetValue, Length: 0);
            else if (source is LongRange r)
            {
                range = r;
            }
            else
            {
                throw new InvalidCastException("Cannot find offset or range type for source delegate");
            }
            return Deserializer.Deserialize(deserializerContext, span, range.Offset, range.Length).Value;
        }

        private object Extract(DeserializerContext context, SourceWithOffset swo)
        {
            LongRange range;
            if (TryCastLong(swo.Offset, out long offsetValue))
            {
                range = new LongRange(Offset: offsetValue, Length: 0);
            }
            else if (swo.Offset is LongRange r)
            {
                range = r;
            }
            else
            {
                throw new InvalidCastException("Cannot find offset or range type for source delegate");
            }
            if (LinearUtil.TryGetReadOnlyMemoryFromPossibleBuffer(swo.Source, out var altMemory))
            {
                return Deserializer.Deserialize(context, altMemory, range.Offset, range.Length).Value;
            }
            throw new InvalidOperationException($"Could not extract memory buffer for {nameof(SourceWithOffset)}");
        }
    }
}
