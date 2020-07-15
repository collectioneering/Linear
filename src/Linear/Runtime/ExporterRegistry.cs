using System.Collections.Generic;

namespace Linear.Runtime
{
    /// <summary>
    /// Stores structures for cross-references
    /// </summary>
    public class ExporterRegistry
    {
        private readonly Dictionary<string, IExporter> _exporters;

        /// <summary>
        /// Create new instance of <see cref="ExporterRegistry"/>
        /// </summary>
        public ExporterRegistry()
        {
            _exporters = new Dictionary<string, IExporter>();
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
