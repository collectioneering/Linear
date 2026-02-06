using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Represents an asynchronous structure evaluation expression.
/// </summary>
public class AsyncStructureEvaluateExpression<T> : ExpressionDefinition
{
    /// <summary>
    /// Delegate type for evaluation expression.
    /// </summary>
    /// <param name="instance">Structure instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public delegate Task<T> AsyncStructureEvaluateDelegate(StructureInstance instance, CancellationToken cancellationToken = default);

    private readonly AsyncStructureEvaluateDelegate _delegate;

    /// <summary>
    /// Initializes an instance of <see cref="StructureEvaluateExpression{T}"/>.
    /// </summary>
    /// <param name="evaluateDelegate">Delegate.</param>
    public AsyncStructureEvaluateExpression(AsyncStructureEvaluateDelegate evaluateDelegate)
    {
        _delegate = evaluateDelegate;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) => [];

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() => new AsyncStructureEvaluateExpressionInstance(_delegate);

    private record AsyncStructureEvaluateExpressionInstance(AsyncStructureEvaluateDelegate Delegate) : ExpressionInstance
    {
        public override object? Evaluate(StructureEvaluationContext context, Stream stream)
        {
            return Delegate(context.Structure).Result;
        }

        public override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, Stream stream, CancellationToken cancellationToken = default)
        {
            return await Delegate(context.Structure, cancellationToken);
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            return Delegate(context.Structure).Result;
        }

        public override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken = default)
        {
            return await Delegate(context.Structure, cancellationToken);
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            return Delegate(context.Structure).Result;
        }
    }
}
