using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Linear.Runtime.Expressions.Operators;

internal abstract record OperatorDualExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
{
    protected abstract object? Evaluate(object? left, object? right);

    public sealed override object? Evaluate(StructureEvaluationContext context, Stream stream)
    {
        object? left = Left.Evaluate(context, stream);
        object? right = Right.Evaluate(context, stream);
        return Evaluate(left, right);
    }

    public sealed override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, Stream stream, CancellationToken cancellationToken = default)
    {
        object? left = await Left.EvaluateAsync(context, stream, cancellationToken);
        object? right = await Right.EvaluateAsync(context, stream, cancellationToken);
        return Evaluate(left, right);
    }

    public sealed override object? Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
    {
        object? left = Left.Evaluate(context, memory);
        object? right = Right.Evaluate(context, memory);
        return Evaluate(left, right);
    }

    public sealed override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken = default)
    {
        object? left = await Left.EvaluateAsync(context, memory, cancellationToken);
        object? right = await Right.EvaluateAsync(context, memory, cancellationToken);
        return Evaluate(left, right);
    }

    public sealed override object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
    {
        object? left = Left.Evaluate(context, span);
        object? right = Right.Evaluate(context, span);
        return Evaluate(left, right);
    }
}
