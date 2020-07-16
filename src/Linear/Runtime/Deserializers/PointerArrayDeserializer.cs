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
        private readonly Type _type;
        private readonly bool _lenFinder;

        /// <summary>
        /// Create new instance of <see cref="ArrayDeserializer"/>
        /// </summary>
        /// <param name="mainDeserializer">Main array deserializer</param>
        /// <param name="elementDeserializer">Element deserializer</param>
        /// <param name="lenFinder">If true, uses space to determine element length (and requires n+1 elements)</param>
        public PointerArrayDeserializer(IDeserializer mainDeserializer, IDeserializer elementDeserializer,
            bool lenFinder)
        {
            _mainDeserializer = mainDeserializer;
            _elementDeserializer = elementDeserializer;
            _type = _elementDeserializer.GetTargetType().MakeArrayType();
            _lenFinder = lenFinder;
        }

        /// <inheritdoc />
        public string GetTargetTypeName() => throw new NotSupportedException();

        /// <inheritdoc />
        public Type GetTargetType() => _type;

        /// <inheritdoc />
        public (object value, long length) Deserialize(StructureInstance instance, Stream stream, byte[] tempBuffer,
            long offset, bool littleEndian, Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            object? pointerOffset = null;
            if (!parameters?.TryGetValue(LinearUtil.PointerOffsetProperty, out pointerOffset) ?? true)
                throw new Exception($"{LinearUtil.PointerOffsetProperty} not specified for array deserializer");
            long rPointerOffset = LinearUtil.CastLong(pointerOffset);
            object? count = null;
            if (!parameters?.TryGetValue(LinearUtil.PointerArrayLengthProperty, out count) ?? true)
                throw new Exception($"{LinearUtil.PointerArrayLengthProperty} not specified for array deserializer");
            long rCount = LinearUtil.CastLong(count);
            (object src, _) = _mainDeserializer.Deserialize(instance, stream, tempBuffer, offset, littleEndian, parameters);
            Array baseArray = (Array)src;
            Array tarArray = Array.CreateInstance(_type, rCount);
            long curOffset = offset;
            for (int i = 0; i < rCount; i++)
            {
                long preElemLength = _lenFinder
                    ? LinearUtil.CastLong(baseArray.GetValue(i + 1)) - LinearUtil.CastLong(baseArray.GetValue(i))
                    : 0;
                (object value, long elemLength) = _elementDeserializer.Deserialize(instance, stream, tempBuffer,
                    rPointerOffset + LinearUtil.CastLong(baseArray.GetValue(i)), littleEndian, parameters,
                    preElemLength, i);
                tarArray.SetValue(value, i);
                curOffset += elemLength;
            }

            return (tarArray, curOffset - offset);
        }
    }
}
