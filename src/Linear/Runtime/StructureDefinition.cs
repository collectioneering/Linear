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
        /// Members not sorted for dependency
        /// </summary>
        public Dictionary<string, Element> Members { get; }

        /// <summary>
        /// Create new instance of <see cref="StructureDefinition"/>
        /// </summary>
        /// <param name="name">Name of structure</param>
        public StructureDefinition(string name)
        {
            Name = name;
            Members = new Dictionary<string, Element>();
        }

        /// <summary>
        /// Build structure layout
        /// </summary>
        /// <returns>Structure</returns>
        public Structure Build()
        {
            List<(string name, Func<StructureInstance, Stream, object> method)> members =
                new List<(string name, Func<StructureInstance, Stream, object> method)>();
            // TODO build members after organizing by dependencies
            return new Structure(Name, members);
        }
    }
}
