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

        internal void Add(string name, Structure structure) => _structures.Add(name, structure);

        /// <summary>
        /// Get structure by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <exception cref="KeyNotFoundException">If structure not found</exception>
        public Structure this[string name] => _structures[name];
    }
}
