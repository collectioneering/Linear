using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime
{
    /// <summary>
    /// Definition of custom deserializer
    /// </summary>
    public abstract class CustomDeserializer
    {
        /// <summary>
        /// Deserialize object
        /// </summary>
        /// <param name="instance">Structure instance</param>
        /// <param name="stream">Stream to read from</param>
        /// <param name="offset">Offset in stream</param>
        /// <param name="littleEndian">Endianness</param>
        /// <param name="parameters">Deserializer parameters</param>
        /// <returns>Deserialized object</returns>
        public abstract object Deserialize(StructureInstance instance, Stream stream, long offset, bool littleEndian, Dictionary<string, object>? parameters);
    }
}
