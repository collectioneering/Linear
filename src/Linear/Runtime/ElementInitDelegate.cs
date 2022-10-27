using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Element init delegate.
/// </summary>
public delegate void ElementInitDelegate(StructureInstance structure, Stream stream, byte[] buffer);
