using System.Collections.Generic;
using Linear.Runtime.Exporters;

namespace Linear.Runtime
{
    /// <summary>
    /// Stores structures for cross-references
    /// </summary>
    public class ExporterRegistry
    {
        private readonly Dictionary<string, IExporter> _exporters;

        private static readonly Dictionary<string, IExporter> _defaultExporters = new Dictionary<string, IExporter>
        {
            {DataExporter.DataExporterName, new DataExporter()}
        };

        /// <summary>
        /// Create default registry with standard exporters
        /// </summary>
        /// <returns>Default registry</returns>
        public static ExporterRegistry CreateDefaultRegistry()
        {
            return new ExporterRegistry(_defaultExporters);
        }

        /// <summary>
        /// Create new instance of <see cref="ExporterRegistry"/>
        /// </summary>
        public ExporterRegistry()
            : this(new Dictionary<string, IExporter>())
        {
        }

        private ExporterRegistry(Dictionary<string, IExporter> exporters)
        {
            _exporters = exporters;
        }

        /// <summary>
        /// Add exporter to registry
        /// </summary>
        /// <param name="exporter">Exporter</param>
        public void Add(IExporter exporter) => _exporters.Add(exporter.GetName(), exporter);

        /// <summary>
        /// Get exporter by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Exporter</returns>
        /// <exception cref="KeyNotFoundException">If exporter found</exception>
        public IExporter this[string name] => _exporters[name];

        /// <summary>
        /// Try to get structure by name
        /// </summary>
        /// <param name="name">Mae</param>
        /// <param name="exporter">Exporter</param>
        /// <returns>True if found</returns>
        public bool TryGetValue(string name, out IExporter exporter) => _exporters.TryGetValue(name, out exporter);
    }
}
