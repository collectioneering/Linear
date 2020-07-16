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
        public string GetTargetTypeName() => throw new NotSupportedException();

        /// <inheritdoc />
        public object Deserialize(StructureInstance instance, Stream stream, byte[] tempBuffer, long offset, bool littleEndian,
            Dictionary<string, object>? parameters, long length = 0)
        {
            return Type.GetTypeCode(_type) switch
            {
                TypeCode.Boolean => LinearUtil.ReadBool(stream, offset, tempBuffer),
                TypeCode.Byte => LinearUtil.ReadS8(stream, offset, tempBuffer),
                TypeCode.Char => LinearUtil.ReadU16(stream, offset, tempBuffer, littleEndian),
                TypeCode.DateTime => throw new NotSupportedException(),
                TypeCode.DBNull => throw new NotSupportedException(),
                TypeCode.Decimal => throw new NotSupportedException(),
                TypeCode.Double => LinearUtil.ReadDouble(stream, offset, tempBuffer),
                TypeCode.Empty => throw new NullReferenceException(),
                TypeCode.Int16 => LinearUtil.ReadS16(stream, offset, tempBuffer, littleEndian),
                TypeCode.Int32 => LinearUtil.ReadS32(stream, offset, tempBuffer, littleEndian),
                TypeCode.Int64 => LinearUtil.ReadS64(stream, offset, tempBuffer, littleEndian),
                TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
                TypeCode.SByte => LinearUtil.ReadS8(stream, offset, tempBuffer),
                TypeCode.Single => LinearUtil.ReadSingle(stream, offset, tempBuffer),
                TypeCode.String => throw new NotSupportedException(), // Not supporting direct
                TypeCode.UInt16 => LinearUtil.ReadU16(stream, offset, tempBuffer, littleEndian),
                TypeCode.UInt32 => LinearUtil.ReadU32(stream, offset, tempBuffer, littleEndian),
                TypeCode.UInt64 => LinearUtil.ReadU64(stream, offset, tempBuffer, littleEndian),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
