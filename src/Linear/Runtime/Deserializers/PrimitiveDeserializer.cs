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
            long offset, bool littleEndian, Dictionary<LinearUtil.StandardProperty, object>? standardProperties,Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            // Possible addition: property group support little endian (requires boolean expressions)
            offset += instance.AbsoluteOffset;
            return Type.GetTypeCode(_type) switch
            {
                TypeCode.Boolean => (LinearUtil.ReadBool(stream, offset, tempBuffer), 1),
                TypeCode.Byte => (LinearUtil.ReadS8(stream, offset, tempBuffer), 1),
                TypeCode.Char => (LinearUtil.ReadU16(stream, offset, tempBuffer, littleEndian), 2),
                TypeCode.DateTime => throw new NotSupportedException(),
                TypeCode.DBNull => throw new NotSupportedException(),
                TypeCode.Decimal => throw new NotSupportedException(),
                TypeCode.Double => (LinearUtil.ReadDouble(stream, offset, tempBuffer), 8),
                TypeCode.Empty => throw new NullReferenceException(),
                TypeCode.Int16 => (LinearUtil.ReadS16(stream, offset, tempBuffer, littleEndian), 2),
                TypeCode.Int32 => (LinearUtil.ReadS32(stream, offset, tempBuffer, littleEndian), 4),
                TypeCode.Int64 => (LinearUtil.ReadS64(stream, offset, tempBuffer, littleEndian), 8),
                TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
                TypeCode.SByte => (LinearUtil.ReadS8(stream, offset, tempBuffer), 1),
                TypeCode.Single => (LinearUtil.ReadSingle(stream, offset, tempBuffer), 4),
                TypeCode.String => throw new NotSupportedException(), // Not supporting direct
                TypeCode.UInt16 => (LinearUtil.ReadU16(stream, offset, tempBuffer, littleEndian), 2),
                TypeCode.UInt32 => (LinearUtil.ReadU32(stream, offset, tempBuffer, littleEndian), 4),
                TypeCode.UInt64 => (LinearUtil.ReadU64(stream, offset, tempBuffer, littleEndian), 8),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
