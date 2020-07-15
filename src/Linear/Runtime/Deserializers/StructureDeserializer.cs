using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Deserializers
{
    /// <summary>
    /// Deserializes structure
    /// </summary>
    public class StructureDeserializer : Deserializer
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
        public override object Deserialize(StructureInstance instance, Stream stream, long offset, bool littleEndian,
            Dictionary<string, object>? parameters, int length = 0)
        {
            return instance.Registry[_name].Parse(instance.Registry, stream, offset, length, instance);
        }
    }
}
