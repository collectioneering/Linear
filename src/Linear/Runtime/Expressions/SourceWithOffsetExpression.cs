using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linear.Utility;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Expression representing a source with an offset.
/// </summary>
public class SourceWithOffsetExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _sourceExpression;
    private readonly ExpressionDefinition _offsetExpression;

    /// <summary>
    /// Initializes an instance of <see cref="SourceWithOffsetExpression"/>.
    /// </summary>
    /// <param name="sourceExpression">Source expression.</param>
    /// <param name="offsetExpression">Offset expression.</param>
    public SourceWithOffsetExpression(ExpressionDefinition sourceExpression, ExpressionDefinition offsetExpression)
    {
        _sourceExpression = sourceExpression;
        _offsetExpression = offsetExpression;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return _sourceExpression.GetDependencies(definition).Union(_offsetExpression.GetDependencies(definition));
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        return new SourceWithOffsetExpressionInstance(_sourceExpression.GetInstance(), _offsetExpression.GetInstance());
    }

    private record SourceWithOffsetExpressionInstance(ExpressionInstance Source, ExpressionInstance Offset) : ExpressionInstance
    {
        public override object Evaluate(StructureInstance structure, Stream stream)
        {
            object source = Source.Evaluate(structure, stream) ?? throw new InvalidOperationException($"{nameof(SourceWithOffset)} source is null");
            long offset = CastUtil.CastLong(Offset.Evaluate(structure, stream));
            return new SourceWithOffset(source, offset);
        }

        public override object Evaluate(StructureInstance structure, ReadOnlySpan<byte> span)
        {
            object source = Source.Evaluate(structure, span) ?? throw new InvalidOperationException($"{nameof(SourceWithOffset)} source is null");
            long offset = CastUtil.CastLong(Offset.Evaluate(structure, span));
            return new SourceWithOffset(source, offset);
        }
    }
}
