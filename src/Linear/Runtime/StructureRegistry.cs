using System.Collections.Generic;

namespace Linear.Runtime
{
    /// <summary>
    /// Stores structures for cross-references
    /// </summary>
    public class StructureRegistry
    {
        private readonly Dictionary<string, Structure> _structures;

        /// <summary>
        /// Create new instance of <see cref="StructureRegistry"/>
        /// </summary>
        public StructureRegistry()
        {
            _structures = new Dictionary<string, Structure>();
        }

        /// <summary>
        /// Add structure to registry
        /// </summary>
        /// <param name="structure">Structure</param>
        public void Add(Structure structure) => _structures.Add(structure.Name, structure);

        /// <summary>
        /// Get structure by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Structure</returns>
        /// <exception cref="KeyNotFoundException">If structure not found</exception>
        public Structure this[string name] => _structures[name];

        /// <summary>
        /// Try to get structure by name
        /// </summary>
        /// <param name="name">Mae</param>
        /// <param name="structure">Structure</param>
        /// <returns>True if found</returns>
        public bool TryGetValue(string name, out Structure? structure) => _structures.TryGetValue(name, out structure);
    }
}
