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

        private readonly List<(string name, Func<StructureInstance, Stream, object> method)> _members;

        /// <summary>
        /// Create new instance of <see cref="Structure"/>
        /// </summary>
        /// <param name="name">Name of structure</param>
        /// <param name="members"></param>
        public Structure(string name, List<(string name, Func<StructureInstance, Stream, object> method)> members)
        {
            Name = name;
            _members = members;
        }

        /// <summary>
        /// Parse structure from stream
        /// </summary>
        /// <param name="registry">Structure registry</param>
        /// <param name="stream">Stream to read from</param>
        /// <param name="offset">Offset in stream</param>
        /// <param name="length">Length of structure</param>
        /// <param name="parent">Parent object</param>
        /// <returns>Parsed structure</returns>
        public StructureInstance Parse(StructureRegistry registry, Stream stream, long offset, long length,
            StructureInstance? parent)
        {
            StructureInstance instance = new StructureInstance(registry, parent, offset, length);
            foreach ((string name, Func<StructureInstance, Stream, object> method) in _members)
                instance.SetMember(name, method(instance, stream));
            return instance;
        }
    }
}
