using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Proxy member expression
/// </summary>
public class ProxyMemberExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _source;
    private readonly string _name;

    /// <summary>
    /// Create new instance of <see cref="ProxyMemberExpression"/>
    /// </summary>
    /// <param name="name">Member name</param>
    /// <param name="source">Source expression</param>
    public ProxyMemberExpression(string name, ExpressionDefinition source)
    {
        _name = name;
        _source = source;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return _source.GetDependencies(definition);
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        return new ProxyMemberExpressionInstance(_source.GetInstance(), _name);
    }

    private record ProxyMemberExpressionInstance(ExpressionInstance Delegate, string Name) : ExpressionInstance
    {
        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            object? val = Delegate.Evaluate(context, stream);
            if (val is StructureInstance i2)
            {
                return i2[Name];
            }
            throw new InvalidCastException($"Could not cast object of type {val?.GetType().FullName} to {nameof(StructureInstance)}");
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            object? val = Delegate.Evaluate(context, memory);
            if (val is StructureInstance i2)
            {
                return i2[Name];
            }
            throw new InvalidCastException($"Could not cast object of type {val?.GetType().FullName} to {nameof(StructureInstance)}");
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            object? val = Delegate.Evaluate(context, span);
            if (val is StructureInstance i2)
            {
                return i2[Name];
            }
            throw new InvalidCastException($"Could not cast object of type {val?.GetType().FullName} to {nameof(StructureInstance)}");
        }
    }
}
