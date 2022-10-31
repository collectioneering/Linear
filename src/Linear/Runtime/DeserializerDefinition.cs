using System.Collections.Generic;

namespace Linear.Runtime;

/// <summary>
/// Base class for deserializer definitions.
/// </summary>
public abstract class DeserializerDefinition
{
    /// <summary>
    /// Gets dependencies.
    /// </summary>
    /// <param name="definition">Structure definition.</param>
    /// <returns>Dependencies.</returns>
    public abstract IEnumerable<Element> GetDependencies(StructureDefinition definition);

    /// <summary>
    /// Gets deserializer instance.
    /// </summary>
    /// <returns>Deserializer.</returns>
    public abstract IDeserializer GetInstance();
}
