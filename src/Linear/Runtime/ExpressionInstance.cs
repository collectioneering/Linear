using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Linear.Runtime;

/// <summary>
/// Expression instance.
/// </summary>
public abstract record ExpressionInstance
{
    /// <summary>
    /// Evaluates expression.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="stream">Stream.</param>
    /// <returns>Result of evaluation.</returns>
    public abstract object? Evaluate(StructureEvaluationContext context, Stream stream);

    /// <summary>
    /// Evaluates expression.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="stream">Stream.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of evaluation.</returns>
    public virtual ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, Stream stream, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(Evaluate(context, stream));
    }

    /// <summary>
    /// Evaluates expression.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="memory">Memory.</param>
    /// <returns>Result of evaluation.</returns>
    public abstract object? Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory);

    /// <summary>
    /// Evaluates expression.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="memory">Memory.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of evaluation.</returns>
    public virtual ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(Evaluate(context, memory));
    }

    /// <summary>
    /// Evaluates expression.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="span">Span.</param>
    /// <returns>Result of evaluation.</returns>
    public abstract object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span);
}
