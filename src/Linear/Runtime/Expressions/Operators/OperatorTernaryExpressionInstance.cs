using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Linear.Utility;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorTernaryExpressionInstance(ExpressionInstance Expression, ExpressionInstance ExpressionTrue, ExpressionInstance ExpressionFalse) : ExpressionInstance
{
    public override object? Evaluate(StructureEvaluationContext context, Stream stream)
    {
        return CastUtil.CastBool(Expression.Evaluate(context, stream))
            ? ExpressionTrue.Evaluate(context, stream)
            : ExpressionFalse.Evaluate(context, stream);
    }

    public override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, Stream stream, CancellationToken cancellationToken = default)
    {
        return CastUtil.CastBool(await Expression.EvaluateAsync(context, stream, cancellationToken))
            ? await ExpressionTrue.EvaluateAsync(context, stream, cancellationToken)
            : await ExpressionFalse.EvaluateAsync(context, stream, cancellationToken);
    }

    public override object? Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
    {
        return CastUtil.CastBool(Expression.Evaluate(context, memory))
            ? ExpressionTrue.Evaluate(context, memory)
            : ExpressionFalse.Evaluate(context, memory);
    }

    public override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken = default)
    {
        return CastUtil.CastBool(await Expression.EvaluateAsync(context, memory, cancellationToken))
            ? await ExpressionTrue.EvaluateAsync(context, memory, cancellationToken)
            : await ExpressionFalse.EvaluateAsync(context, memory, cancellationToken);
    }

    public override object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
    {
        return CastUtil.CastBool(Expression.Evaluate(context, span))
            ? ExpressionTrue.Evaluate(context, span)
            : ExpressionFalse.Evaluate(context, span);
    }
}
