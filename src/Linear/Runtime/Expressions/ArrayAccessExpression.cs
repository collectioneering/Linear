using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Create new instance of <see cref="ProxyMemberExpression"/>
    /// </summary>
    /// <param name="source">Source expression</param>
    /// <param name="index">Index expression</param>
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
            object? source = Source.Evaluate(context, stream);
            object index = Index.Evaluate(context, stream) ?? throw new NullReferenceException();
            if (source is Array sourceValue)
            {
                return sourceValue.GetValue(CastUtil.CastInt(index));
            }
            throw new InvalidCastException($"Could not cast object of type {source?.GetType().FullName} to {nameof(Array)}");
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            object? source = Source.Evaluate(context, memory);
            object index = Index.Evaluate(context, memory) ?? throw new NullReferenceException();
            if (source is Array sourceValue)
            {
                return sourceValue.GetValue(CastUtil.CastInt(index));
            }
            throw new InvalidCastException($"Could not cast object of type {source?.GetType().FullName} to {nameof(Array)}");
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            object? source = Source.Evaluate(context, span);
            object index = Index.Evaluate(context, span) ?? throw new NullReferenceException();
            if (source is Array sourceValue)
            {
                return sourceValue.GetValue(CastUtil.CastInt(index));
            }
            throw new InvalidCastException($"Could not cast object of type {source?.GetType().FullName} to {nameof(Array)}");
        }
    }
}
