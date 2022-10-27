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

        private readonly List<StructureMember> _members;

        /// <summary>
        /// Create new instance of <see cref="Structure"/>
        /// </summary>
        /// <param name="name">Name of structure</param>
        /// <param name="defaultLength">Default length of structure</param>
        /// <param name="members"></param>
        public Structure(string name, int defaultLength, List<StructureMember> members)
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
        public StructureInstance Parse(StructureRegistry registry, Stream stream, long offset = 0, StructureInstance? parent = null, long length = 0, int index = 0)
        {
            StructureInstance instance = new(registry, parent, offset, length == 0 ? DefaultLength : length, index);
            foreach ((string? _, ElementInitializer method) in _members)
            {
                method.Initialize(instance, stream);
            }

            return instance;
        }
    }

    /// <summary>
    /// Represents a structure member.
    /// </summary>
    /// <param name="Name">Member name.</param>
    /// <param name="Initializer">Initializer.</param>
    public readonly record struct StructureMember(string? Name, ElementInitializer Initializer);
}
