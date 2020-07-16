using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Deserializers
{
    /// <summary>
    /// Generic string deserializer
    /// </summary>
    public class StringDeserializer : IDeserializer
    {
        /// <summary>
        /// Deserializer mode
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Fixed-length UTF-8
            /// </summary>
            Utf8Fixed,

            /// <summary>
            /// Null-terminated UTF-8
            /// </summary>
            Utf8Null,

            /// <summary>
            /// Fixed-length UTF-16
            /// </summary>
            Utf16Fixed,

            /// <summary>
            /// Null-terminated UTF-16
            /// </summary>
            Utf16Null
        }

        private readonly Mode _mode;

        /// <summary>
        /// Create new instance of <see cref="StringDeserializer"/>
        /// </summary>
        /// <param name="mode">Deserializer mode</param>
        public StringDeserializer(Mode mode)
        {
            _mode = mode;
        }

        /// <inheritdoc />
        public string? GetTargetTypeName() => null;

        /// <inheritdoc />
        public Type GetTargetType() => typeof(string);

        /// <inheritdoc />
        public (object value, long length) Deserialize(StructureInstance instance, Stream stream, byte[] tempBuffer, long offset,
            bool littleEndian, Dictionary<string, object>? parameters, long length = 0, int index = 0)
        {
            // TODO implement string deserializer
            throw new NotImplementedException();
        }
    }
}
