using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    private readonly Dictionary<LinearCommon.StandardProperty, ExpressionDefinition> _standardProperties;

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
        Dictionary<LinearCommon.StandardProperty, ExpressionDefinition> standardProperties)
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
        Dictionary<LinearCommon.StandardProperty, ExpressionDefinition> standardProperties)
    {
        ExpressionInstance srcDelegate = offsetDefinition.GetInstance();
        ExpressionInstance littleEndianDelegate = littleEndianDefinition.GetInstance();
        Dictionary<string, ExpressionInstance> deserializerParamsCompact = new();
        foreach (var kvp in deserializerParams)
            deserializerParamsCompact[kvp.Key] = kvp.Value.GetInstance();
        Dictionary<LinearCommon.StandardProperty, ExpressionInstance>
            standardPropertiesCompact = new();
        foreach (var kvp in standardProperties)
            standardPropertiesCompact[kvp.Key] = kvp.Value.GetInstance();
        return new DeserializeExpressionInstance(deserializerParamsCompact, standardPropertiesCompact, srcDelegate, littleEndianDelegate, deserializer);
    }

    private record DeserializeExpressionInstance(
        Dictionary<string, ExpressionInstance> DeserializerParamsCompact,
        Dictionary<LinearCommon.StandardProperty, ExpressionInstance> StandardPropertiesCompact,
        ExpressionInstance Source, ExpressionInstance LittleEndian, IDeserializer Deserializer) : ExpressionInstance
    {
        public override object Evaluate(StructureInstance structure, Stream stream)
        {
            Dictionary<string, object>? deserializerParamsGen =
                DeserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
            if (deserializerParamsGen != null)
                foreach (var kvp in DeserializerParamsCompact)
                    deserializerParamsGen[kvp.Key] = kvp.Value.Evaluate(structure, stream) ?? throw new NullReferenceException();

            Dictionary<LinearCommon.StandardProperty, object>? standardPropertiesGen =
                StandardPropertiesCompact.Count != 0
                    ? new Dictionary<LinearCommon.StandardProperty, object>()
                    : null;
            if (standardPropertiesGen != null)
                foreach (var kvp in StandardPropertiesCompact)
                    standardPropertiesGen[kvp.Key] = kvp.Value.Evaluate(structure, stream) ?? throw new NullReferenceException();
            object? offset = Source.Evaluate(structure, stream);
            object? littleEndian = LittleEndian.Evaluate(structure, stream);
            LongRange range;
            if (LinearCommon.TryCastLong(offset, out long offsetValue))
                range = new LongRange(Offset: offsetValue, Length: 0);
            else if (offset is LongRange r)
            {
                range = r;
            }
            else
            {
                throw new InvalidCastException("Cannot find offset or range type for source delegate");
            }

            if (littleEndian is bool littleEndianValue)
            {
                return Deserializer.Deserialize(structure, stream, range.Offset, littleEndianValue, standardPropertiesGen, deserializerParamsGen, range.Length).Value;
            }
            throw new InvalidCastException($"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
        }
    }
}
