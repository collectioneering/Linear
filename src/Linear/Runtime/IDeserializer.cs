using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime
{
    /// <summary>
    /// Definition of custom deserializer
    /// </summary>
    public interface IDeserializer
    {
        /// <summary>
        /// Get target type name of deserializer
        /// </summary>
        /// <returns>Exporter name</returns>
        string GetTargetTypeName();

        /// <summary>
        /// Deserialize object
        /// </summary>
        /// <param name="instance">Structure instance</param>
        /// <param name="stream">Stream to read from</param>
        /// <param name="offset">Offset in stream</param>
        /// <param name="littleEndian">Endianness</param>
        /// <param name="parameters">Deserializer parameters</param>
        /// <param name="length">Length of structure</param>
        /// <returns>Deserialized object</returns>
        public abstract object Deserialize(StructureInstance instance, Stream stream, long offset, bool littleEndian,
            Dictionary<string, object>? parameters, int length = 0);
    }
}
