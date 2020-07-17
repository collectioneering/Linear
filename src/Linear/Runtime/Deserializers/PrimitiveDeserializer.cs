using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Deserializers
{
    /// <summary>
    /// Deserializes primitive
    /// </summary>
    public class PrimitiveDeserializer : IDeserializer
    {
        private readonly Type _type;

        /// <summary>
        /// Create new instance of <see cref="PrimitiveDeserializer"/>
        /// </summary>
        /// <param name="type">Target type</param>
        public PrimitiveDeserializer(Type type)
        {
            _type = type;
        }

        /// <inheritdoc />
        public string? GetTargetTypeName() => null;

        /// <inheritdoc />
        public Type GetTargetType() => _type;

        /// <inheritdoc />
        public (object value, long length) Deserialize(StructureInstance instance, Stream stream, byte[] tempBuffer,
            long offset, bool littleEndian, Dictionary<LinearCommon.StandardProperty, object>? standardProperties,
            Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            // Possible addition: property group support little endian (requires boolean expressions)
            offset += instance.AbsoluteOffset;
            return Type.GetTypeCode(_type) switch
            {
                TypeCode.Boolean => (LinearCommon.ReadBool(stream, offset, tempBuffer), 1),
                TypeCode.Byte => (LinearCommon.ReadS8(stream, offset, tempBuffer), 1),
                TypeCode.Char => (LinearCommon.ReadU16(stream, offset, tempBuffer, littleEndian), 2),
                TypeCode.DateTime => throw new NotSupportedException(),
                TypeCode.DBNull => throw new NotSupportedException(),
                TypeCode.Decimal => throw new NotSupportedException(),
                TypeCode.Double => (LinearCommon.ReadDouble(stream, offset, tempBuffer), 8),
                TypeCode.Empty => throw new NullReferenceException(),
                TypeCode.Int16 => (LinearCommon.ReadS16(stream, offset, tempBuffer, littleEndian), 2),
                TypeCode.Int32 => (LinearCommon.ReadS32(stream, offset, tempBuffer, littleEndian), 4),
                TypeCode.Int64 => (LinearCommon.ReadS64(stream, offset, tempBuffer, littleEndian), 8),
                TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
                TypeCode.SByte => (LinearCommon.ReadS8(stream, offset, tempBuffer), 1),
                TypeCode.Single => (LinearCommon.ReadSingle(stream, offset, tempBuffer), 4),
                TypeCode.String => throw new NotSupportedException(), // Not supporting direct
                TypeCode.UInt16 => (LinearCommon.ReadU16(stream, offset, tempBuffer, littleEndian), 2),
                TypeCode.UInt32 => (LinearCommon.ReadU32(stream, offset, tempBuffer, littleEndian), 4),
                TypeCode.UInt64 => (LinearCommon.ReadU64(stream, offset, tempBuffer, littleEndian), 8),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
