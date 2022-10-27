using System;
using System.Collections.Generic;
using System.Linq;

namespace Linear.Runtime.Elements
{
    /// <summary>
    /// Expression from offset
    /// </summary>
    public class OutputElement : Element
    {
        private readonly ExpressionDefinition _formatDefinition;
        private readonly ExpressionDefinition _rangeDefinition;
        private readonly ExpressionDefinition _nameDefinition;
        private readonly Dictionary<string, ExpressionDefinition>? _exporterParams;

        /// <summary>
        /// Create new instance of <see cref="OutputElement"/>
        /// </summary>
        /// <param name="formatDefinition">Format value definition</param>
        /// <param name="rangeDefinition">Range value definition</param>
        /// <param name="nameDefinition">Name value definition</param>
        /// <param name="exporterParams">Exporter parameters</param>
        public OutputElement(ExpressionDefinition formatDefinition, ExpressionDefinition rangeDefinition,
            ExpressionDefinition nameDefinition, Dictionary<string, ExpressionDefinition>? exporterParams)
        {
            _formatDefinition = formatDefinition;
            _rangeDefinition = rangeDefinition;
            _nameDefinition = nameDefinition;
            _exporterParams = exporterParams;
        }

        /// <inheritdoc />
        public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
        {
            return _rangeDefinition.GetDependencies(definition)
                .Union(_nameDefinition.GetDependencies(definition));
        }

        /// <inheritdoc />
        public override ElementInitDelegate GetDelegate()
        {
            ExpressionInstance formatDelegate = _formatDefinition.GetInstance();
            ExpressionInstance rangeDelegate = _rangeDefinition.GetInstance();
            ExpressionInstance nameDelegate = _nameDefinition.GetInstance();
            Dictionary<string, ExpressionInstance>? exporterParamsCompact = _exporterParams == null ? null : new Dictionary<string, ExpressionInstance>();
            if (_exporterParams != null)
            {
                foreach (var kvp in _exporterParams)
                    exporterParamsCompact![kvp.Key] = kvp.Value.GetInstance();
            }

            return (instance, stream) =>
            {
                object? format = formatDelegate.Deserialize(instance, stream);
                object? range = rangeDelegate.Deserialize(instance, stream);
                object? name = nameDelegate.Deserialize(instance, stream);
                Dictionary<string, object>? exporterParams =
                    exporterParamsCompact == null ? null : new Dictionary<string, object>();
                if (exporterParamsCompact != null)
                    foreach (var kvp in exporterParamsCompact)
                        exporterParams![kvp.Key] =
                            kvp.Value.Deserialize(instance, stream) ?? throw new NullReferenceException();
                if (!LinearCommon.TryCast(format, out string formatValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {format?.GetType().FullName} to type {nameof(String)}");
                if (!LinearCommon.TryCast(range, out LongRange rangeValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {range?.GetType().FullName} to type {nameof(LongRange)}");
                instance.AddOutput((name?.ToString() ?? instance.GetUniqueId().ToString(), formatValue, exporterParams,
                    rangeValue));
            };
        }
    }
}
