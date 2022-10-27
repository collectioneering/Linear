using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Deserializers
{
    /// <summary>
    /// Deserializes pointer array
    /// </summary>
    public class PointerArrayDeserializer : IDeserializer
    {
        private readonly IDeserializer _mainDeserializer;
        private readonly IDeserializer _elementDeserializer;
        private readonly Type _elementType;
        private readonly Type _type;
        private readonly bool _lenFinder;

        /// <summary>
        /// Create new instance of <see cref="ArrayDeserializer"/>
        /// </summary>
        /// <param name="mainDeserializer">Main array deserializer</param>
        /// <param name="elementDeserializer">Element deserializer</param>
        /// <param name="lenFinder">If true, uses space to determine element length (and requires n+1 elements)</param>
        public PointerArrayDeserializer(IDeserializer mainDeserializer, IDeserializer elementDeserializer, bool lenFinder)
        {
            _mainDeserializer = mainDeserializer;
            _elementDeserializer = elementDeserializer;
            _elementType = _elementDeserializer.GetTargetType();
            _type = _elementType.MakeArrayType();
            _lenFinder = lenFinder;
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
            (object src, _) = _mainDeserializer.Deserialize(instance, stream, offset, littleEndian, standardProperties, parameters);
            Array baseArray = (Array)src;
            int pointerArrayLength = LinearCommon.CastInt(standardProperties[LinearCommon.StandardProperty.PointerArrayLengthProperty]);
            long pointerOffset = LinearCommon.CastLong(standardProperties[LinearCommon.StandardProperty.PointerOffsetProperty]);
            Array tarArray = Array.CreateInstance(_elementType, pointerArrayLength);
            long curOffset = offset;
            for (int i = 0; i < pointerArrayLength; i++)
            {
                long vI = LinearCommon.CastLong(baseArray.GetValue(i));
                long preElemLength = _lenFinder ? LinearCommon.CastLong(baseArray.GetValue(i + 1)) - vI : 0;
                (object value, long elemLength) = _elementDeserializer.Deserialize(instance, stream, pointerOffset + vI, littleEndian, standardProperties, parameters, preElemLength, i);
                tarArray.SetValue(value, i);
                curOffset += elemLength;
            }

            return new DeserializeResult(tarArray, curOffset - offset);
        }
    }
}
