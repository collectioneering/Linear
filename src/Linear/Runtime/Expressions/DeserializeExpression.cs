using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions
{
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
        public override Func<StructureInstance, Stream, byte[], object?> GetDelegate() => CreateDelegate(
            _offsetDefinition, _littleEndianDefinition, _deserializer, _deserializerParams, _standardProperties);

        internal static Func<StructureInstance, Stream, byte[], object?> CreateDelegate(
            ExpressionDefinition offsetDefinition,
            ExpressionDefinition littleEndianDefinition, IDeserializer deserializer,
            Dictionary<string, ExpressionDefinition> deserializerParams,
            Dictionary<LinearCommon.StandardProperty, ExpressionDefinition> standardProperties)
        {
            Func<StructureInstance, Stream, byte[], object?> srcDelegate = offsetDefinition.GetDelegate();
            Func<StructureInstance, Stream, byte[], object?>
                littleEndianDelegate = littleEndianDefinition.GetDelegate();
            Dictionary<string, Func<StructureInstance, Stream, byte[], object?>> deserializerParamsCompact =
                new Dictionary<string, Func<StructureInstance, Stream, byte[], object?>>();
            foreach (var kvp in deserializerParams)
                deserializerParamsCompact[kvp.Key] = kvp.Value.GetDelegate();
            Dictionary<LinearCommon.StandardProperty, Func<StructureInstance, Stream, byte[], object?>>
                standardPropertiesCompact =
                    new Dictionary<LinearCommon.StandardProperty, Func<StructureInstance, Stream, byte[], object?>>();
            foreach (var kvp in standardProperties)
                standardPropertiesCompact[kvp.Key] = kvp.Value.GetDelegate();
            return (instance, stream, tempBuffer) =>
            {
                Dictionary<string, object>? deserializerParamsGen =
                    deserializerParamsCompact.Count != 0 ? new Dictionary<string, object>() : null;
                if (deserializerParamsGen != null)
                    foreach (var kvp in deserializerParamsCompact)
                        deserializerParamsGen[kvp.Key] =
                            kvp.Value(instance, stream, tempBuffer) ?? throw new NullReferenceException();

                Dictionary<LinearCommon.StandardProperty, object>? standardPropertiesGen =
                    standardPropertiesCompact.Count != 0
                        ? new Dictionary<LinearCommon.StandardProperty, object>()
                        : null;
                if (standardPropertiesGen != null)
                    foreach (var kvp in standardPropertiesCompact)
                        standardPropertiesGen[kvp.Key] =
                            kvp.Value(instance, stream, tempBuffer) ?? throw new NullReferenceException();
                object? offset = srcDelegate(instance, stream, tempBuffer);
                object? littleEndian = littleEndianDelegate(instance, stream, tempBuffer);
                (long offset, long length) range = default;
                if (LinearCommon.TryCastLong(offset, out long offsetValue))
                    range.offset = offsetValue;
                else if (!LinearCommon.TryCast(offset, out range))
                    throw new InvalidCastException("Cannot find offset or range type for source delegate");

                if (!LinearCommon.TryCast(littleEndian, out bool littleEndianValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
                return deserializer.Deserialize(instance, stream, tempBuffer, range.offset,
                    littleEndianValue, standardPropertiesGen, deserializerParamsGen, range.length).value;
            };
        }
    }
}
