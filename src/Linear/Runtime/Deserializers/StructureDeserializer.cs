using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Deserializers
{
    /// <summary>
    /// Deserializes structure
    /// </summary>
    public class StructureDeserializer : IDeserializer
    {
        private readonly string _name;

        /// <summary>
        /// Create new instance of <see cref="StructureDeserializer"/>
        /// </summary>
        /// <param name="name">Name of structure</param>
        public StructureDeserializer(string name)
        {
            _name = name;
        }

        /// <inheritdoc />
        public string? GetTargetTypeName() => _name;

        /// <inheritdoc />
        public Type GetTargetType() => typeof(StructureInstance);

        /// <inheritdoc />
        public (object value, long length) Deserialize(StructureInstance instance, Stream stream, byte[] tempBuffer, long offset,
            bool littleEndian, Dictionary<LinearCommon.StandardProperty, object>? standardProperties,Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            StructureInstance i = instance.Registry[_name].Parse(instance.Registry, stream, offset, instance, length, index);
            return (i, i.Length);
        }
    }
}
