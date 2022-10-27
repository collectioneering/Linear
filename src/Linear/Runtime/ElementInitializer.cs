using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Initializer for element.
/// </summary>
public abstract record ElementInitializer()
{
    /// <summary>
    /// Initializes element.
    /// </summary>
    /// <param name="structure">Structure.</param>
    /// <param name="stream">Stream.</param>
    public abstract void Initialize(StructureInstance structure, Stream stream);
}
