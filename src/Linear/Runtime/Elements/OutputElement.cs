using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Elements;

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
        return _rangeDefinition.GetDependencies(definition).Union(_nameDefinition.GetDependencies(definition));
    }

    /// <inheritdoc />
    public override ElementInitializer GetInitializer()
    {
        Dictionary<string, ExpressionInstance>? exporterParamsCompact = _exporterParams == null ? null : new Dictionary<string, ExpressionInstance>();
        if (_exporterParams != null)
        {
            foreach (var kvp in _exporterParams)
            {
                exporterParamsCompact![kvp.Key] = kvp.Value.GetInstance();
            }
        }
        return new OutputElementInitializer(_formatDefinition.GetInstance(), _rangeDefinition.GetInstance(), _nameDefinition.GetInstance(), exporterParamsCompact);
    }

    private record OutputElementInitializer(ExpressionInstance Format, ExpressionInstance Range, ExpressionInstance Name,
        Dictionary<string, ExpressionInstance>? ExporterParamsCompact) : ElementInitializer
    {
        public override void Initialize(StructureInstance structure, Stream stream)
        {
            object? format = Format.Evaluate(structure, stream);
            object? range = Range.Evaluate(structure, stream);
            object? name = Name.Evaluate(structure, stream);
            Dictionary<string, object>? exporterParams = ExporterParamsCompact == null ? null : new Dictionary<string, object>();
            if (ExporterParamsCompact != null)
            {
                foreach (var kvp in ExporterParamsCompact)
                {
                    exporterParams![kvp.Key] = kvp.Value.Evaluate(structure, stream) ?? throw new NullReferenceException();
                }
            }
            if (format is not string formatValue)
            {
                throw new InvalidCastException($"Could not cast expression of type {format?.GetType().FullName} to type {nameof(String)}");
            }
            if (range is not LongRange rangeValue)
            {
                throw new InvalidCastException($"Could not cast expression of type {range?.GetType().FullName} to type {nameof(LongRange)}");
            }
            structure.AddOutput(new StructureOutput(structure, name?.ToString() ?? structure.GetUniqueId().ToString(CultureInfo.InvariantCulture), formatValue, exporterParams, rangeValue));
        }
    }
}
