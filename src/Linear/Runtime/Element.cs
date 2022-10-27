using System.Collections.Generic;

namespace Linear.Runtime
{
    /// <summary>
    /// Represents element in body of structure
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Determine dependencies on other members in structure
        /// </summary>
        /// <param name="definition">Structure to use</param>
        /// <returns>Dependencies</returns>
        /// <remarks>Does not resolve references to parent</remarks>
        public abstract IEnumerable<Element> GetDependencies(StructureDefinition definition);

        /// <summary>
        /// Get initializer
        /// </summary>
        /// <returns>Initializer</returns>
        public abstract ElementInitializer GetInitializer();
    }
}
