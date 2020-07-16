using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var sub = new List<(string?, Element)>(Members);
            // Build members after organizing by dependencies
            while (sub.Count > 0)
            {
                int removed = sub.RemoveAll(e =>
                {
                    bool noDeps = !e.Item2.GetDependencies(this).Intersect(sub.Select(x => x.Item2)).Any();
                    if (noDeps) members.Add((e.Item1, e.Item2.GetDelegate()));
                    return noDeps;
                });
                if (removed == 0) throw new Exception("Failed to reduce dependencies");
            }

            return new Structure(Name, DefaultLength, members);
        }
    }
}
