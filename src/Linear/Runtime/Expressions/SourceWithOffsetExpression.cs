using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            object source = Source.Evaluate(context, stream) ?? throw new InvalidOperationException($"{nameof(SourceWithOffset)} source is null");
            object offset = Offset.Evaluate(context, stream) ?? throw new InvalidOperationException($"{nameof(SourceWithOffset)} offset is null");
            return new SourceWithOffset(source, offset);
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            object source = Source.Evaluate(context, memory) ?? throw new InvalidOperationException($"{nameof(SourceWithOffset)} source is null");
            object offset = Offset.Evaluate(context, memory) ?? throw new InvalidOperationException($"{nameof(SourceWithOffset)} offset is null");
            return new SourceWithOffset(source, offset);
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            object source = Source.Evaluate(context, span) ?? throw new InvalidOperationException($"{nameof(SourceWithOffset)} source is null");
            object offset = Offset.Evaluate(context, span) ?? throw new InvalidOperationException($"{nameof(SourceWithOffset)} offset is null");
            return new SourceWithOffset(source, offset);
        }
    }
}
