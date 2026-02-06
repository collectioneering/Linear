using System.Collections.Generic;

namespace Linear.Runtime;

/// <summary>
/// Represents element in body of structure
/// </summary>
public abstract class Element
{
    /// <summary>
    /// Determines dependencies on other members in structure.
    /// </summary>
    /// <param name="definition">Structure to use.</param>
    /// <returns>Dependencies.</returns>
    /// <remarks>Does not resolve references to parent.</remarks>
    public abstract IEnumerable<Element> GetDependencies(StructureDefinition definition);

    /// <summary>
    /// Gets initializer.
    /// </summary>
    /// <returns>Initializer.</returns>
    public abstract ElementInitializer GetInitializer();
}
