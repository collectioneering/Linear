using System.Collections.Generic;
using Linear.Runtime;

namespace Linear
{
    /// <summary>
    /// ANTLR listener implementation
    /// </summary>
    public class LinearListener : LinearBaseListener
    {
        private readonly List<StructureDefinition> _structures;

        /// <summary>
        /// Create new instance of <see cref="LinearListener"/>
        /// </summary>
        public LinearListener()
        {
            _structures = new List<StructureDefinition>();
        }

        /// <summary>
        /// Get parsed structures
        /// </summary>
        /// <returns>Structures</returns>
        public List<StructureDefinition> GetStructures() => new List<StructureDefinition>(_structures);

        // TODO implement listener
    }
}
