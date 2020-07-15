using System;
using System.Collections.Generic;
using System.IO;
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
        public override Action<StructureInstance, Stream, byte[]> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object?> formatDelegate = _formatDefinition.GetDelegate();
            Func<StructureInstance, Stream, byte[], object?> rangeDelegate = _rangeDefinition.GetDelegate();
            Func<StructureInstance, Stream, byte[], object?> nameDelegate = _nameDefinition.GetDelegate();
            Dictionary<string, Func<StructureInstance, Stream, byte[], object?>>? exporterParamsCompact =
                _exporterParams == null
                    ? null
                    : new Dictionary<string, Func<StructureInstance, Stream, byte[], object?>>();
            if (_exporterParams != null)
            {
                foreach (var kvp in _exporterParams)
                    exporterParamsCompact![kvp.Key] = kvp.Value.GetDelegate();
            }

            return (instance, stream, tempBuffer) =>
            {
                object? format = formatDelegate(instance, stream, tempBuffer);
                object? range = rangeDelegate(instance, stream, tempBuffer);
                object? name = nameDelegate(instance, stream, tempBuffer);
                Dictionary<string, object>? exporterParams =
                    exporterParamsCompact == null ? null : new Dictionary<string, object>();
                if (exporterParamsCompact != null)
                    foreach (var kvp in exporterParamsCompact)
                        exporterParams![kvp.Key] =
                            kvp.Value(instance, stream, tempBuffer) ?? throw new NullReferenceException();
                if (!LinearUtil.TryCast(format, out string formatValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {format?.GetType().FullName} to type {nameof(String)}");
                if (!LinearUtil.TryCast(range, out (long, long) rangeValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {range?.GetType().FullName} to type {nameof(ValueTuple<long, long>)}");
                if (!LinearUtil.TryCast(name, out string nameValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {name?.GetType().FullName} to type {nameof(String)}");
                instance.AddOutput((nameValue, formatValue, exporterParams, rangeValue));
            };
        }
    }
}
