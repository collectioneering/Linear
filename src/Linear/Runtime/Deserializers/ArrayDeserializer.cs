using System;
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
        private readonly Type _type;

        /// <summary>
        /// Create new instance of <see cref="ArrayDeserializer"/>
        /// </summary>
        /// <param name="elementDeserializer">Element deserializer</param>
        public ArrayDeserializer(IDeserializer elementDeserializer)
        {
            _elementDeserializer = elementDeserializer;
            _type = _elementDeserializer.GetTargetType().MakeArrayType();
        }

        /// <inheritdoc />
        public string GetTargetTypeName() => throw new NotSupportedException();

        /// <inheritdoc />
        public Type GetTargetType() => _type;

        /// <inheritdoc />
        public (object value, long length) Deserialize(StructureInstance instance, Stream stream, byte[] tempBuffer,
            long offset, bool littleEndian, Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            object? count = null;
            if (parameters?.TryGetValue(LinearUtil.ArrayLengthProperty, out count) ?? false)
                throw new Exception($"{LinearUtil.ArrayLengthProperty} not specified for array deserializer");
            int rCount = LinearUtil.CastInt(count);
            Array res = Array.CreateInstance(_type, rCount);
            long curOffset = offset;
            for (int i = 0; i < rCount; i++)
            {
                (object value, long elemLength) = _elementDeserializer.Deserialize(instance, stream, tempBuffer,
                    curOffset, littleEndian, parameters, 0, i);
                res.SetValue(value, i);
                curOffset += elemLength;
            }

            return (res, curOffset - offset);
        }
    }
}
