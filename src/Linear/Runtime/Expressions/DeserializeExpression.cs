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
    private readonly Dictionary<string, ExpressionDefinition> _deserializerParams;
    private readonly Dictionary<StandardProperty, ExpressionDefinition> _standardProperties;

    /// <summary>
    /// Create new instance of <see cref="DeserializeExpression"/>
    /// </summary>
    /// <param name="offsetDefinition">Offset value definition</param>
    /// <param name="littleEndianDefinition">Endianness value definition</param>
    /// <param name="deserializer">Custom deserializer</param>
    /// <param name="deserializerParams">Deserializer parameters</param>
    /// <param name="standardProperties">Standard property expressions</param>
    public DeserializeExpression(ExpressionDefinition offsetDefinition, ExpressionDefinition littleEndianDefinition,
        IDeserializer deserializer, Dictionary<string, ExpressionDefinition> deserializerParams,
        Dictionary<StandardProperty, ExpressionDefinition> standardProperties)
    {
        _offsetDefinition = offsetDefinition;
        _littleEndianDefinition = littleEndianDefinition;
        _deserializer = deserializer;
        _deserializerParams = deserializerParams;
        _standardProperties = standardProperties;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return _offsetDefinition.GetDependencies(definition)
            .Union(_littleEndianDefinition.GetDependencies(definition))
            .Union(_deserializerParams.SelectMany(kvp => kvp.Value.GetDependencies(definition)))
            .Union(_standardProperties.SelectMany(kvp => kvp.Value.GetDependencies(definition)));
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() =>
        CreateDelegate(_offsetDefinition, _littleEndianDefinition, _deserializer, _deserializerParams, _standardProperties);

    internal static ExpressionInstance CreateDelegate(
        ExpressionDefinition offsetDefinition,
        ExpressionDefinition littleEndianDefinition, IDeserializer deserializer,
        Dictionary<string, ExpressionDefinition> deserializerParams,
        Dictionary<StandardProperty, ExpressionDefinition> standardProperties)
    {
        ExpressionInstance srcDelegate = offsetDefinition.GetInstance();
        ExpressionInstance littleEndianDelegate = littleEndianDefinition.GetInstance();
        Dictionary<string, ExpressionInstance> deserializerParamsCompact = new();
        foreach (var kvp in deserializerParams)
            deserializerParamsCompact[kvp.Key] = kvp.Value.GetInstance();
        Dictionary<StandardProperty, ExpressionInstance>
            standardPropertiesCompact = new();
        foreach (var kvp in standardProperties)
            standardPropertiesCompact[kvp.Key] = kvp.Value.GetInstance();
        return new DeserializeExpressionInstance(deserializerParamsCompact, standardPropertiesCompact, srcDelegate, littleEndianDelegate, deserializer);
    }

    private record DeserializeExpressionInstance(
        Dictionary<string, ExpressionInstance> DeserializerParamsCompact,
        Dictionary<StandardProperty, ExpressionInstance> StandardPropertiesCompact,
        ExpressionInstance Source, ExpressionInstance LittleEndian, IDeserializer Deserializer) : ExpressionInstance
    {
        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, stream) ?? throw new NullReferenceException();
            Dictionary<StandardProperty, object>? standardPropertiesGen = StandardPropertiesCompact.Count != 0 ? new Dictionary<StandardProperty, object>() : null;
            if (standardPropertiesGen != null)
                foreach (var kvp in StandardPropertiesCompact)
                    standardPropertiesGen[kvp.Key] = kvp.Value.Evaluate(context, stream) ?? throw new NullReferenceException();
            object? littleEndian = LittleEndian.Evaluate(context, stream);
            if (littleEndian is not bool littleEndianValue)
            {
                throw new InvalidCastException($"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
            }
            object? source = Source.Evaluate(context, stream);
            if (source is SourceWithOffset swo)
            {
                return Extract(context, swo, littleEndianValue, deserializerParamsGen, standardPropertiesGen);
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
            return Deserializer.Deserialize(context.Structure, stream, range.Offset, littleEndianValue, standardPropertiesGen, deserializerParamsGen, range.Length).Value;
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, memory) ?? throw new NullReferenceException();
            Dictionary<StandardProperty, object>? standardPropertiesGen = StandardPropertiesCompact.Count != 0 ? new Dictionary<StandardProperty, object>() : null;
            if (standardPropertiesGen != null)
                foreach (var kvp in StandardPropertiesCompact)
                    standardPropertiesGen[kvp.Key] = kvp.Value.Evaluate(context, memory) ?? throw new NullReferenceException();
            object? littleEndian = LittleEndian.Evaluate(context, memory);
            if (littleEndian is not bool littleEndianValue)
            {
                throw new InvalidCastException($"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
            }
            object? source = Source.Evaluate(context, memory);
            if (source is SourceWithOffset swo)
            {
                return Extract(context, swo, littleEndianValue, deserializerParamsGen, standardPropertiesGen);
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
            return Deserializer.Deserialize(context.Structure, memory, range.Offset, littleEndianValue, standardPropertiesGen, deserializerParamsGen, range.Length).Value;
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            Dictionary<string, object>? deserializerParamsGen = DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(context, span) ?? throw new NullReferenceException();
            Dictionary<StandardProperty, object>? standardPropertiesGen = StandardPropertiesCompact.Count != 0 ? new Dictionary<StandardProperty, object>() : null;
            if (standardPropertiesGen != null)
                foreach (var kvp in StandardPropertiesCompact)
                    standardPropertiesGen[kvp.Key] = kvp.Value.Evaluate(context, span) ?? throw new NullReferenceException();
            object? littleEndian = LittleEndian.Evaluate(context, span);
            if (littleEndian is not bool littleEndianValue)
            {
                throw new InvalidCastException($"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
            }
            object? source = Source.Evaluate(context, span);
            if (source is SourceWithOffset swo)
            {
                return Extract(context, swo, littleEndianValue, deserializerParamsGen, standardPropertiesGen);
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
            return Deserializer.Deserialize(context.Structure, span, range.Offset, littleEndianValue, standardPropertiesGen, deserializerParamsGen, range.Length).Value;
        }

        private object Extract(StructureEvaluationContext context, SourceWithOffset swo, bool littleEndianValue, Dictionary<string, object>? deserializerParamsGen, Dictionary<StandardProperty, object>? standardPropertiesGen)
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
                return Deserializer.Deserialize(context.Structure, altMemory, range.Offset, littleEndianValue, standardPropertiesGen, deserializerParamsGen, range.Length).Value;
            }
            throw new InvalidOperationException($"Could not extract memory buffer for {nameof(SourceWithOffset)}");
        }
    }
}
