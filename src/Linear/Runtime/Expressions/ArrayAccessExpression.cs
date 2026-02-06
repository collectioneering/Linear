using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Linear.Utility;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Member expression
/// </summary>
public class ArrayAccessExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _source;
    private readonly ExpressionDefinition _index;

    /// <summary>
    /// Initializes an instance of <see cref="ProxyMemberExpression"/>.
    /// </summary>
    /// <param name="source">Source expression.</param>
    /// <param name="index">Index expression.</param>
    public ArrayAccessExpression(ExpressionDefinition source, ExpressionDefinition index)
    {
        _source = source;
        _index = index;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _source.GetDependencies(definition).Union(_index.GetDependencies(definition));

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() => new ArrayAccessExpressionInstance(_source.GetInstance(), _index.GetInstance());

    private record ArrayAccessExpressionInstance(ExpressionInstance Source, ExpressionInstance Index) : ExpressionInstance
    {
        public override object? Evaluate(StructureEvaluationContext context, Stream stream)
        {
            return Evaluate(Source.Evaluate(context, stream), Index.Evaluate(context, stream));
        }

        public override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, Stream stream, CancellationToken cancellationToken = default)
        {
            return Evaluate(await Source.EvaluateAsync(context, stream, cancellationToken), await Index.EvaluateAsync(context, stream, cancellationToken));
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            return Evaluate(Source.Evaluate(context, memory), Index.Evaluate(context, memory));
        }

        public override async ValueTask<object?> EvaluateAsync(StructureEvaluationContext context, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken = default)
        {
            return Evaluate(await Source.EvaluateAsync(context, memory, cancellationToken), await Index.EvaluateAsync(context, memory, cancellationToken));
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            return Evaluate(Source.Evaluate(context, span), Index.Evaluate(context, span));
        }

        private static object? Evaluate(object? left, object? right)
        {
            ArgumentNullException.ThrowIfNull(right);
            if (left is Array sourceValue)
            {
                return sourceValue.GetValue(CastUtil.CastInt(right));
            }
            throw new InvalidCastException($"Could not cast object of type {left?.GetType().FullName} to {nameof(Array)}");
        }
    }
}
