using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Exporters;

/// <summary>
/// Decompression proxy delegate.
/// </summary>
public delegate Stream DecompressionProxyDelegate(Stream stream, IReadOnlyDictionary<string, object> parameters);
