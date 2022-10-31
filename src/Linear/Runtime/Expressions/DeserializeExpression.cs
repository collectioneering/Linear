using System;
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
    private readonly ExpressionDefinition _littleEndianDefinition;
    private readonly IDeserializer _deserializer;
    private readonly DeserializerStandardProperties _standardProperties;
    private readonly Dictionary<string, ExpressionDefinition> _deserializerParams;

    /// <summary>
    /// Create new instance of <see cref="DeserializeExpression"/>
    /// </summary>
    /// <param name="offsetDefinition">Offset value definition</param>
    /// <param name="littleEndianDefinition">Endianness value definition</param>
    /// <param name="deserializer">Custom deserializer</param>
    /// <param name="deserializerParams">Deserializer parameters</param>
    /// <param name="standardProperties">Standard property expressions</param>
    public DeserializeExpression(ExpressionDefinition offsetDefinition, ExpressionDefinition littleEndianDefinition,
        IDeserializer deserializer, DeserializerStandardProperties standardProperties, Dictionary<string, ExpressionDefinition> deserializerParams)
    {
        _offsetDefinition = offsetDefinition;
        _littleEndianDefinition = littleEndianDefinition;
        _deserializer = deserializer;
        _standardProperties = standardProperties;
        _deserializerParams = deserializerParams;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return _offsetDefinition.GetDependencies(definition)
            .Union(_littleEndianDefinition.GetDependencies(definition))
            .Union(_deserializerParams.SelectMany(kvp => kvp.Value.GetDependencies(definition)))
            .Union(_standardProperties.GetDependencies(definition));
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() =>
        CreateDelegate(_offsetDefinition, _littleEndianDefinition, _deserializer, _standardProperties, _deserializerParams);

    internal static ExpressionInstance CreateDelegate(
        ExpressionDefinition offsetDefinition,
        ExpressionDefinition littleEndianDefinition, IDeserializer deserializer,
        DeserializerStandardProperties standardProperties,
        Dictionary<string, ExpressionDefinition> deserializerParams)
    {
        ExpressionInstance srcDelegate = offsetDefinition.GetInstance();
        ExpressionInstance littleEndianDelegate = littleEndianDefinition.GetInstance();
        Dictionary<string, ExpressionInstance> deserializerParamsCompact = new();
        foreach (var kvp in deserializerParams)
            deserializerParamsCompact[kvp.Key] = kvp.Value.GetInstance();
        return new DeserializeExpressionInstance(standardProperties.ToInstance(), deserializerParamsCompact, srcDelegate, littleEndianDelegate, deserializer);
    }

    private record DeserializeExpressionInstance(
        DeserializerStandardPropertiesInstance StandardPropertiesCompact,
        Dictionary<string, ExpressionInstance> DeserializerParamsCompact,
        ExpressionInstance Source, ExpressionInstance LittleEndian, IDeserializer Deserializer) : ExpressionInstance
    {
        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, stream) ?? throw new NullReferenceException();
            var deserializerContext = new DeserializerContext(context.Structure, deserializerParamsGen);
            deserializerContext = StandardPropertiesCompact.Augment(deserializerContext, context, stream);
            object? littleEndian = LittleEndian.Evaluate(context, stream);
            if (littleEndian is not bool littleEndianValue)
            {
                throw new InvalidCastException($"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
            }
            object? source = Source.Evaluate(context, stream);
            if (source is SourceWithOffset swo)
            {
                return Extract(deserializerContext, swo, littleEndianValue);
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
            return Deserializer.Deserialize(deserializerContext, stream, range.Offset, littleEndianValue, range.Length).Value;
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, memory) ?? throw new NullReferenceException();
            var deserializerContext = new DeserializerContext(context.Structure, deserializerParamsGen);
            deserializerContext = StandardPropertiesCompact.Augment(deserializerContext, context, memory);
            object? littleEndian = LittleEndian.Evaluate(context, memory);
            if (littleEndian is not bool littleEndianValue)
            {
                throw new InvalidCastException($"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
            }
            object? source = Source.Evaluate(context, memory);
            if (source is SourceWithOffset swo)
            {
                return Extract(deserializerContext, swo, littleEndianValue);
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
            return Deserializer.Deserialize(deserializerContext, memory, range.Offset, littleEndianValue, range.Length).Value;
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, span) ?? throw new NullReferenceException();
            var deserializerContext = new DeserializerContext(context.Structure, deserializerParamsGen);
            deserializerContext = StandardPropertiesCompact.Augment(deserializerContext, context, span);
            object? littleEndian = LittleEndian.Evaluate(context, span);
            if (littleEndian is not bool littleEndianValue)
            {
                throw new InvalidCastException($"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
            }
            object? source = Source.Evaluate(context, span);
            if (source is SourceWithOffset swo)
            {
                return Extract(deserializerContext, swo, littleEndianValue);
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
            return Deserializer.Deserialize(deserializerContext, span, range.Offset, littleEndianValue, range.Length).Value;
        }

        private object Extract(DeserializerContext context, SourceWithOffset swo, bool littleEndianValue)
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
                return Deserializer.Deserialize(context, altMemory, range.Offset, littleEndianValue, range.Length).Value;
            }
            throw new InvalidOperationException($"Could not extract memory buffer for {nameof(SourceWithOffset)}");
        }
    }
}
