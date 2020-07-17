using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Linear.Runtime.Exporters
{
    /// <summary>
    /// Decompression exporter
    /// </summary>
    public class DecompressExporter : IExporter
    {
        static DecompressExporter()
        {
            SupportedDecompressors = new Dictionary<string, Func<Stream, Dictionary<string, object>, Stream>>();
            SupportedDecompressors["gzip"] =
                (stream, configuration) => new GZipStream(stream, CompressionMode.Decompress);
            SupportedDecompressors["deflate"] =
                (stream, configuration) => new DeflateStream(stream, CompressionMode.Decompress);
        }

        /// <summary>
        /// Supported decompressors
        /// </summary>
        public static readonly Dictionary<string, Func<Stream, Dictionary<string, object>, Stream>>
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
        public void Export(Stream stream, StructureInstance instance, (long offset, long length) range,
            Dictionary<string, object>? parameters, Stream outputStream)
        {
            stream.Position = instance.AbsoluteOffset + range.offset;
            using SStream sStream = new SStream(stream, range.length);
            if (parameters == null) throw new Exception("Parameters cannot be null");
            if (!parameters.TryGetValue(Key_Format, out object format) || !(format is string formatString))
                throw new Exception($"Required key {ExporterName} missing");
            if (!SupportedDecompressors.TryGetValue(formatString,
                out Func<Stream, Dictionary<string, object>, Stream> fn))
                throw new Exception($"Unknown format {format}");
            Stream proxyStream = fn(sStream, parameters);
            proxyStream.CopyTo(outputStream);
        }
    }
}
