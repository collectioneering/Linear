using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Linear.Utility.CastUtil;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Range expression
/// </summary>
public class RangeExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _startExpression;
    private readonly ExpressionDefinition? _endExpression;
    private readonly ExpressionDefinition? _lengthExpression;

    /// <summary>
    /// Create new instance of <see cref="RangeExpression"/>
    /// </summary>
    /// <param name="startExpression">Start offset expression</param>
    /// <param name="endExpression">End offset expression</param>
    /// <param name="lengthExpression">Length expression</param>
    public RangeExpression(ExpressionDefinition startExpression, ExpressionDefinition? endExpression,
        ExpressionDefinition? lengthExpression)
    {
        _startExpression = startExpression;
        _endExpression = endExpression;
        _lengthExpression = lengthExpression;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        IEnumerable<Element> deps = _startExpression.GetDependencies(definition);
        if (_endExpression != null) deps = deps.Union(_endExpression.GetDependencies(definition));
        if (_lengthExpression != null) deps = deps.Union(_lengthExpression.GetDependencies(definition));
        return deps;
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        ExpressionInstance startDelegate = _startExpression.GetInstance();
        if (_endExpression != null)
        {
            ExpressionInstance endDelegate = _endExpression.GetInstance();
            return new RangeExpressionInstanceStartEnd(startDelegate, endDelegate);
        }
        if (_lengthExpression != null)
        {
            ExpressionInstance lengthDelegate = _lengthExpression!.GetInstance();
            return new RangeExpressionInstanceStartLength(startDelegate, lengthDelegate);
        }
        throw new InvalidOperationException();
    }

    private record RangeExpressionInstanceStartEnd(ExpressionInstance Start, ExpressionInstance End) : ExpressionInstance
    {
        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            long start = CastLong(Start.Evaluate(context, stream));
            long end = CastLong(End.Evaluate(context, stream));
            return new LongRange(start, end - start);
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            long start = CastLong(Start.Evaluate(context, memory));
            long end = CastLong(End.Evaluate(context, memory));
            return new LongRange(start, end - start);
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            long start = CastLong(Start.Evaluate(context, span));
            long end = CastLong(End.Evaluate(context, span));
            return new LongRange(start, end - start);
        }
    }

    private record RangeExpressionInstanceStartLength(ExpressionInstance Start, ExpressionInstance Length) : ExpressionInstance
    {
        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            long start = CastLong(Start.Evaluate(context, stream));
            long length = CastLong(Length.Evaluate(context, stream));
            return new LongRange(start, length);
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            long start = CastLong(Start.Evaluate(context, memory));
            long length = CastLong(Length.Evaluate(context, memory));
            return new LongRange(start, length);
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            long start = CastLong(Start.Evaluate(context, span));
            long length = CastLong(Length.Evaluate(context, span));
            return new LongRange(start, length);
        }
    }
}
