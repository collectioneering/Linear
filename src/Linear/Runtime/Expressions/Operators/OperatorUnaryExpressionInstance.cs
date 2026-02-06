using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Linear.Runtime.Expressions.Operators;

internal abstract record OperatorUnaryExpressionInstance(ExpressionInstance Expression) : ExpressionInstance
{
    public override object Evaluate(StructureEvaluationContext context, Stream stream)
    {
        return Evaluate(Expression.Evaluate(context, stream));
    }

    public override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, Stream stream, CancellationToken cancellationToken = default)
    {
        return Evaluate(await Expression.EvaluateAsync(context, stream, cancellationToken));
    }

    public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
    {
        return Evaluate(Expression.Evaluate(context, memory));
    }

    public override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken = default)
    {
        return Evaluate(await Expression.EvaluateAsync(context, memory, cancellationToken));
    }

    public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
    {
        return Evaluate(Expression.Evaluate(context, span));
    }

    protected abstract object Evaluate(object? value);
}
