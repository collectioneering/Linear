using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Lambda replacement expression.
/// </summary>
public class LambdaReplacementExpression : ExpressionDefinition
{
    private readonly string _name;

    /// <summary>
    /// Initializes an instance of <see cref="LambdaReplacementExpression"/>.
    /// </summary>
    /// <param name="name">Variable name.</param>
    public LambdaReplacementExpression(string name)
    {
        _name = name;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return Enumerable.Empty<Element>();
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() => new LambdaReplacementExpressionInstance(_name);

    private record LambdaReplacementExpressionInstance(string Name) : ExpressionInstance
    {
        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            if (context.LambdaReplacements == null)
            {
                throw new InvalidOperationException("No lambda replacements available");
            }
            if (context.LambdaReplacements.TryGetValue(Name, out object? obj))
            {
                return obj;
            }
            throw new InvalidOperationException($"Could not find lambda replacement {Name}");
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            if (context.LambdaReplacements == null)
            {
                throw new InvalidOperationException("No lambda replacements available");
            }
            if (context.LambdaReplacements.TryGetValue(Name, out object? obj))
            {
                return obj;
            }
            throw new InvalidOperationException($"Could not find lambda replacement {Name}");
        }
    }
}
