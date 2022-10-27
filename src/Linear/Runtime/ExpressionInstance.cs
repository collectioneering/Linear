using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Expression instance.
/// </summary>
public abstract record ExpressionInstance
{
    /// <summary>
    /// Deserialize expression.
    /// </summary>
    /// <param name="structure">Structure.</param>
    /// <param name="stream">Stream.</param>
    /// <returns>Deserialized object.</returns>
    public abstract object? Deserialize(StructureInstance structure, Stream stream);
}
