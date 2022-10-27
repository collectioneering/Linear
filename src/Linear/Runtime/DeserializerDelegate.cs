using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Deserializer delegate.
/// </summary>
public delegate object? DeserializerDelegate(StructureInstance structure, Stream stream);
