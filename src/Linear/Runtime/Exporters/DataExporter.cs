using System.Collections.Generic;
using System.IO;
using Esper;

namespace Linear.Runtime.Exporters
{
    /// <summary>
    /// Data exporter
    /// </summary>
    public class DataExporter : IExporter
    {
        /// <summary>
        /// Name of data exporter
        /// </summary>
        public const string DataExporterName = "data";

        /// <inheritdoc />
        public string GetName() => DataExporterName;

        /// <inheritdoc />
        public void Export(Stream stream, StructureInstance instance, (long offset, long length) range,
            Dictionary<string, object>? parameters, Stream outputStream)
        {
            stream.Position = instance.AbsoluteOffset + range.offset;
            using SStream sStream = new SStream(stream, range.length);
            sStream.CopyTo(outputStream);
        }
    }
}
