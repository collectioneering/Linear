using System.Collections.Generic;

namespace Linear.Runtime;

/// <summary>
/// Definition of expression
/// </summary>
public abstract class ExpressionDefinition
{
    /// <summary>
    /// Determines dependencies on other members in structure.
    /// </summary>
    /// <param name="definition">Structure to use.</param>
    /// <returns>Dependencies.</returns>
    /// <remarks>Does not resolve references to parent.</remarks>
    public abstract IEnumerable<Element> GetDependencies(StructureDefinition definition);

    /// <summary>
    /// Gets instance.
    /// </summary>
    /// <returns>Instance.</returns>
    public abstract ExpressionInstance GetInstance();
}
