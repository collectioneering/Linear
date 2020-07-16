using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime
{
    /// <summary>
    /// Definition of structure
    /// </summary>
    public class StructureDefinition
    {
        /// <summary>
        /// Name of structure
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Default length of structure
        /// </summary>
        public int DefaultLength { get; }

        /// <summary>
        /// Members not sorted for dependency
        /// </summary>
        public List<(string?, Element)> Members { get; }

        /// <summary>
        /// Create new instance of <see cref="StructureDefinition"/>
        /// </summary>
        /// <param name="name">Name of structure</param>
        /// <param name="defaultLength">Default length of structure</param>
        public StructureDefinition(string name, int defaultLength)
        {
            Name = name;
            DefaultLength = defaultLength;
            Members = new List<(string?, Element)>();
        }

        /// <summary>
        /// Build structure layout
        /// </summary>
        /// <returns>Structure</returns>
        public Structure Build()
        {
            List<(string? name, Action<StructureInstance, Stream, byte[]> method)> members =
                new List<(string? name, Action<StructureInstance, Stream, byte[]> method)>();
            // TODO build members after organizing by dependencies
            return new Structure(Name, DefaultLength, members);
        }
    }
}
