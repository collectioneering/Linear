using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime
{
    /// <summary>
    /// Structure layout
    /// </summary>
    public class Structure
    {
        /// <summary>
        /// Name of structure
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Default length of structure
        /// </summary>
        public int DefaultLength { get; }

        private readonly List<(string? name, Action<StructureInstance, Stream, byte[]> method)> _members;

        /// <summary>
        /// Create new instance of <see cref="Structure"/>
        /// </summary>
        /// <param name="name">Name of structure</param>
        /// <param name="defaultLength">Default length of structure</param>
        /// <param name="members"></param>
        public Structure(string name, int defaultLength,
            List<(string? name, Action<StructureInstance, Stream, byte[]> method)> members)
        {
            Name = name;
            _members = members;
            DefaultLength = defaultLength;
        }

        /// <summary>
        /// Parse structure from stream
        /// </summary>
        /// <param name="registry">Structure registry</param>
        /// <param name="stream">Stream to read from</param>
        /// <param name="offset">Offset in stream</param>
        /// <param name="parent">Parent object</param>
        /// <param name="length">Length of structure</param>
        /// <param name="index">Array index</param>
        /// <returns>Parsed structure</returns>
        public StructureInstance Parse(StructureRegistry registry, Stream stream, long offset = 0,
            StructureInstance? parent = null, long length = 0, int index = 0)
        {
            byte[] tempBuf = new byte[sizeof(ulong)];
            StructureInstance instance =
                new StructureInstance(registry, parent, offset, length == 0 ? DefaultLength : length, index);
            foreach ((string? _, Action<StructureInstance, Stream, byte[]> method) in _members)
            {
                method(instance, stream, tempBuf);
            }

            return instance;
        }
    }
}
