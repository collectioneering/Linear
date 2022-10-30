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

    // TODO this requires additional context during call to evaluate lambda replacements

    private record LambdaReplacementExpressionInstance(string Name) : ExpressionInstance
    {
        public override object Evaluate(StructureInstance structure, Stream stream)
        {
            throw new NotImplementedException();
        }

        public override object Evaluate(StructureInstance structure, ReadOnlySpan<byte> span)
        {
            throw new NotImplementedException();
        }
    }
}
