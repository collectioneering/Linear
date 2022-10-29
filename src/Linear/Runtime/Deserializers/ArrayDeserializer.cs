﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Deserializers
{
    /// <summary>
    /// Deserializes raw array
    /// </summary>
    public class ArrayDeserializer : IDeserializer
    {
        private readonly IDeserializer _elementDeserializer;
        private readonly Type _elementType;
        private readonly Type _type;

        /// <summary>
        /// Create new instance of <see cref="ArrayDeserializer"/>
        /// </summary>
        /// <param name="elementDeserializer">Element deserializer</param>
        public ArrayDeserializer(IDeserializer elementDeserializer)
        {
            _elementDeserializer = elementDeserializer;
            _elementType = _elementDeserializer.GetTargetType();
            _type = _elementType.MakeArrayType();
        }

        /// <inheritdoc />
        public string GetTargetTypeName() => throw new NotSupportedException();

        /// <inheritdoc />
        public Type GetTargetType() => _type;

        /// <inheritdoc />
        public DeserializeResult Deserialize(StructureInstance instance, Stream stream,
            long offset, bool littleEndian, Dictionary<LinearCommon.StandardProperty, object>? standardProperties,
            Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            if (standardProperties == null) throw new NullReferenceException();
            int arrayLength = CastUtil.CastInt(standardProperties[LinearCommon.StandardProperty.ArrayLengthProperty]);
            Array res = Array.CreateInstance(_elementType, arrayLength);
            long curOffset = offset;
            for (int i = 0; i < arrayLength; i++)
            {
                (object value, long elemLength) = _elementDeserializer.Deserialize(instance, stream,
                    curOffset, littleEndian, standardProperties, parameters, 0, i);
                res.SetValue(value, i);
                curOffset += elemLength;
            }

            return new DeserializeResult(res, curOffset - offset);
        }
    }
}
