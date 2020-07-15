using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions
{
    /// <summary>
    /// Expression from offset
    /// </summary>
    public class OffsetDataExpression : ExpressionDefinition
    {
        private readonly ExpressionDefinition _offsetDefinition;
        private readonly ExpressionDefinition _littleEndianDefinition;
        private readonly CustomDeserializer? _deserializer;
        private readonly Dictionary<string, ExpressionDefinition>? _deserializerParams;

        /// <summary>
        /// Create new instance of <see cref="OffsetDataExpression"/>
        /// </summary>
        /// <param name="type">Expression type</param>
        /// <param name="offsetDefinition">Offset value definition</param>
        /// <param name="littleEndianDefinition">Endianness value definition</param>
        public OffsetDataExpression(Type type, ExpressionDefinition offsetDefinition,
            ExpressionDefinition littleEndianDefinition)
            : base(type)
        {
            _offsetDefinition = offsetDefinition;
            _littleEndianDefinition = littleEndianDefinition;
            _deserializer = null;
        }

        /// <summary>
        /// Create new instance of <see cref="OffsetDataExpression"/>
        /// </summary>
        /// <param name="type">Expression type</param>
        /// <param name="offsetDefinition">Offset value definition</param>
        /// <param name="littleEndianDefinition">Endianness value definition</param>
        /// <param name="deserializer">Custom deserializer</param>
        /// <param name="deserializerParams">Deserialier parameters</param>
        public OffsetDataExpression(Type type, ExpressionDefinition offsetDefinition,
            ExpressionDefinition littleEndianDefinition, CustomDeserializer deserializer,
            Dictionary<string, ExpressionDefinition> deserializerParams)
            : base(type)
        {
            _offsetDefinition = offsetDefinition;
            _littleEndianDefinition = littleEndianDefinition;
            _deserializer = deserializer;
            _deserializerParams = deserializerParams;
        }

        /// <inheritdoc />
        public override List<ExpressionDefinition> GetDependencies(StructureDefinition definition)
        {
            IEnumerable<ExpressionDefinition> deps = _offsetDefinition.GetDependencies(definition)
                .Union(_littleEndianDefinition.GetDependencies(definition));
            if (_deserializerParams != null) deps = deps.Union(_deserializerParams.Select(kvp => kvp.Value));
            return deps.ToList();
        }

        /// <inheritdoc />
        public override Func<StructureInstance, Stream, byte[], object> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object> srcDelegate = _offsetDefinition.GetDelegate();
            Func<StructureInstance, Stream, byte[], object>
                littleEndianDelegate = _littleEndianDefinition.GetDelegate();
            Dictionary<string, Func<StructureInstance, Stream, byte[], object>> deserializerParamsCompact =
                new Dictionary<string, Func<StructureInstance, Stream, byte[], object>>();
            if (_deserializer != null && _deserializerParams != null)
            {
                foreach (var kvp in _deserializerParams)
                    deserializerParamsCompact[kvp.Key] = kvp.Value.GetDelegate();
                return (instance, stream, tempBuffer) =>
                {
                    Dictionary<string, object> deserializerParams = new Dictionary<string, object>();
                    foreach (var kvp in deserializerParamsCompact)
                        deserializerParams[kvp.Key] = kvp.Value(instance, stream, tempBuffer);
                    object offset = srcDelegate(instance, stream, tempBuffer);
                    object littleEndian = littleEndianDelegate(instance, stream, tempBuffer);
                    if (!LinearUtil.TryCast(offset, out long offsetValue))
                        throw new InvalidCastException(
                            $"Could not cast expression of type {offset.GetType().FullName} to type {nameof(Int64)}");
                    if (!LinearUtil.TryCast(littleEndian, out bool littleEndianValue))
                        throw new InvalidCastException(
                            $"Could not cast expression of type {littleEndian.GetType().FullName} to type {nameof(Boolean)}");
                    offsetValue += instance.AbsoluteOffset;
                    return _deserializer.Deserialize(instance, stream, offsetValue, littleEndianValue,
                        deserializerParams);
                };
            }

            return (instance, stream, tempBuffer) =>
            {
                object offset = srcDelegate(instance, stream, tempBuffer);
                object littleEndian = littleEndianDelegate(instance, stream, tempBuffer);
                if (!LinearUtil.TryCast(offset, out long offsetValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {offset.GetType().FullName} to type {nameof(Int64)}");
                if (!LinearUtil.TryCast(littleEndian, out bool littleEndianValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {littleEndian.GetType().FullName} to type {nameof(Boolean)}");
                offsetValue += instance.AbsoluteOffset;
                return Type.GetTypeCode(Type) switch
                {
                    TypeCode.Boolean => LinearUtil.ReadBool(stream, offsetValue, tempBuffer),
                    TypeCode.Byte => LinearUtil.ReadS8(stream, offsetValue, tempBuffer),
                    TypeCode.Char => LinearUtil.ReadU16(stream, offsetValue, tempBuffer, littleEndianValue),
                    TypeCode.DateTime => throw new NotSupportedException(),
                    TypeCode.DBNull => throw new NotSupportedException(),
                    TypeCode.Decimal => throw new NotSupportedException(),
                    TypeCode.Double => LinearUtil.ReadDouble(stream, offsetValue, tempBuffer),
                    TypeCode.Empty => throw new NullReferenceException(),
                    TypeCode.Int16 => LinearUtil.ReadS16(stream, offsetValue, tempBuffer, littleEndianValue),
                    TypeCode.Int32 => LinearUtil.ReadS32(stream, offsetValue, tempBuffer, littleEndianValue),
                    TypeCode.Int64 => LinearUtil.ReadS64(stream, offsetValue, tempBuffer, littleEndianValue),
                    TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
                    TypeCode.SByte => LinearUtil.ReadS8(stream, offsetValue, tempBuffer),
                    TypeCode.Single => LinearUtil.ReadSingle(stream, offsetValue, tempBuffer),
                    TypeCode.String => throw new NotSupportedException(), // Not supporting direct
                    TypeCode.UInt16 => LinearUtil.ReadU16(stream, offsetValue, tempBuffer, littleEndianValue),
                    TypeCode.UInt32 => LinearUtil.ReadU32(stream, offsetValue, tempBuffer, littleEndianValue),
                    TypeCode.UInt64 => LinearUtil.ReadU64(stream, offsetValue, tempBuffer, littleEndianValue),
                    _ => throw new ArgumentOutOfRangeException()
                };
            };
        }
    }
}
