using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Fp;
using Linear.Utility;

namespace Linear.Runtime.Exporters;

/// <summary>
/// Decompression exporter
/// </summary>
public class DecompressExporter : IExporter
{
    static DecompressExporter()
    {
        SupportedDecompressors = new Dictionary<string, DecompressionProxyDelegate>();
        SupportedDecompressors["gzip"] = (stream, _) => new GZipStream(stream, CompressionMode.Decompress);
        SupportedDecompressors["deflate"] = (stream, _) => new DeflateStream(stream, CompressionMode.Decompress);
    }

    /// <summary>
    /// Supported decompressors
    /// </summary>
    public static readonly Dictionary<string, DecompressionProxyDelegate>
        SupportedDecompressors;

    /// <summary>
    /// Name of data exporter
    /// </summary>
    public const string ExporterName = "compressed";

    /// <summary>
    /// Format key
    /// </summary>
    public const string Key_Format = "format";

    /// <inheritdoc />
    public string GetName() => ExporterName;

    /// <inheritdoc />
    public void Export(Stream stream, StructureInstance instance, LongRange range,
        IReadOnlyDictionary<string, object>? parameters, Stream outputStream)
    {
        stream.Position = instance.AbsoluteOffset + range.Offset;
        using SStream sStream = new(stream, range.Length);
        if (parameters == null) throw new Exception("Parameters cannot be null");
        if (!parameters.TryGetValue(Key_Format, out object? format) || !(format is string formatString))
            throw new Exception($"Required key {ExporterName} missing");
        if (!SupportedDecompressors.TryGetValue(formatString, out DecompressionProxyDelegate? fn))
            throw new Exception($"Unknown format {format}");
        Stream proxyStream = fn(sStream, parameters);
        proxyStream.CopyTo(outputStream);
    }

    /// <inheritdoc />
    public void Export(ReadOnlyMemory<byte> memory, StructureInstance instance, LongRange range,
        IReadOnlyDictionary<string, object>? parameters, Stream outputStream)
    {
        LinearUtil.TrimExportTarget(ref memory, instance, range);
        if (parameters == null) throw new Exception("Parameters cannot be null");
        if (!parameters.TryGetValue(Key_Format, out object? format) || !(format is string formatString))
            throw new Exception($"Required key {ExporterName} missing");
        if (!SupportedDecompressors.TryGetValue(formatString, out DecompressionProxyDelegate? fn))
            throw new Exception($"Unknown format {format}");
        Stream proxyStream = fn(new MStream(memory), parameters);
        proxyStream.CopyTo(outputStream);
    }

    /// <inheritdoc />
    public unsafe void Export(ReadOnlySpan<byte> span, StructureInstance instance, LongRange range,
        IReadOnlyDictionary<string, object>? parameters, Stream outputStream)
    {
        LinearUtil.TrimExportTarget(ref span, instance, range);
        fixed (byte* p = span)
        {
            if (parameters == null) throw new Exception("Parameters cannot be null");
            if (!parameters.TryGetValue(Key_Format, out object? format) || !(format is string formatString))
                throw new Exception($"Required key {ExporterName} missing");
            if (!SupportedDecompressors.TryGetValue(formatString, out DecompressionProxyDelegate? fn))
                throw new Exception($"Unknown format {format}");
            Stream proxyStream = fn(new PStream(new IntPtr(p), span.Length), parameters);
            proxyStream.CopyTo(outputStream);
        }
    }
}
