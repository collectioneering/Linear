using System;
using System.Collections.Generic;
using System.IO;
using Fp;
using Linear.Utility;

namespace Linear.Runtime.Exporters;

/// <summary>
/// Data exporter
/// </summary>
public class DataExporter : IExporter
{
    /// <summary>
    /// Name of data exporter
    /// </summary>
    public const string ExporterName = "data";

    /// <inheritdoc />
    public string GetName() => ExporterName;

    /// <inheritdoc />
    public void Export(Stream stream, StructureInstance instance, LongRange range,
        IReadOnlyDictionary<string, object>? parameters, Stream outputStream)
    {
        stream.Position = instance.AbsoluteOffset + range.Offset;
        using SStream sStream = new(stream, range.Length);
        sStream.CopyTo(outputStream);
    }

    /// <inheritdoc />
    public void Export(ReadOnlyMemory<byte> memory, StructureInstance instance, LongRange range,
        IReadOnlyDictionary<string, object>? parameters, Stream outputStream)
    {
        LinearUtil.TrimRange(ref memory, instance, range);
        outputStream.Write(memory.Span);
    }

    /// <inheritdoc />
    public void Export(ReadOnlySpan<byte> span, StructureInstance instance, LongRange range,
        IReadOnlyDictionary<string, object>? parameters, Stream outputStream)
    {
        LinearUtil.TrimRange(ref span, instance, range);
        outputStream.Write(span);
    }
}
