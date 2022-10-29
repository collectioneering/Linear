using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Provides common interface for exporters
/// </summary>
public interface IExporter
{
    /// <summary>
    /// Get name of exporter
    /// </summary>
    /// <returns>Exporter name</returns>
    string GetName();

    /// <summary>
    /// Export data
    /// </summary>
    /// <param name="stream">Base stream</param>
    /// <param name="instance">Instance to extract from</param>
    /// <param name="range">Target range</param>
    /// <param name="parameters">Exporter parameters</param>
    /// <param name="outputStream">Output stream</param>
    void Export(Stream stream, StructureInstance instance, LongRange range, IReadOnlyDictionary<string, object>? parameters, Stream outputStream);
}