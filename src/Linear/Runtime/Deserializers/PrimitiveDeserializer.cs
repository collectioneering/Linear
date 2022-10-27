﻿using System;
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

        // TODO return type as record struct

        /// <inheritdoc />
        public DeserializeResult Deserialize(StructureInstance instance, Stream stream,
            long offset, bool littleEndian, Dictionary<LinearCommon.StandardProperty, object>? standardProperties,
            Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            // Possible addition: property group support little endian (requires boolean expressions)
            offset += instance.AbsoluteOffset;
            return Type.GetTypeCode(_type) switch
            {
                TypeCode.Boolean => new DeserializeResult(LinearCommon.ReadBool(stream, offset), 1),
                TypeCode.Byte => new DeserializeResult(LinearCommon.ReadU8(stream, offset), 1),
                TypeCode.Char => new DeserializeResult(LinearCommon.ReadU16(stream, offset, littleEndian), 2),
                TypeCode.DateTime => throw new NotSupportedException(),
                TypeCode.DBNull => throw new NotSupportedException(),
                TypeCode.Decimal => throw new NotSupportedException(),
                TypeCode.Double => new DeserializeResult(LinearCommon.ReadDouble(stream, offset), 8),
                TypeCode.Empty => throw new NullReferenceException(),
                TypeCode.Int16 => new DeserializeResult(LinearCommon.ReadS16(stream, offset, littleEndian), 2),
                TypeCode.Int32 => new DeserializeResult(LinearCommon.ReadS32(stream, offset, littleEndian), 4),
                TypeCode.Int64 => new DeserializeResult(LinearCommon.ReadS64(stream, offset, littleEndian), 8),
                TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
                TypeCode.SByte => new DeserializeResult(LinearCommon.ReadS8(stream, offset), 1),
                TypeCode.Single => new DeserializeResult(LinearCommon.ReadSingle(stream, offset), 4),
                TypeCode.String => throw new NotSupportedException(), // Not supporting direct
                TypeCode.UInt16 => new DeserializeResult(LinearCommon.ReadU16(stream, offset, littleEndian), 2),
                TypeCode.UInt32 => new DeserializeResult(LinearCommon.ReadU32(stream, offset, littleEndian), 4),
                TypeCode.UInt64 => new DeserializeResult(LinearCommon.ReadU64(stream, offset, littleEndian), 8),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
