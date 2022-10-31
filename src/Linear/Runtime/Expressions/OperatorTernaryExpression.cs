using System;
using System.Collections.Generic;
using System.Linq;
using Linear.Runtime.Expressions.Operators;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Ternary operator expression.
/// </summary>
public class OperatorTernaryExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _expression;
    private readonly ExpressionDefinition _expressionTrue;
    private readonly ExpressionDefinition _expressionFalse;

    /// <summary>
    /// Initializes an instance of <see cref="OperatorTernaryExpression"/>.
    /// </summary>
    /// <param name="expression">Expression.</param>
    /// <param name="expressionTrue">Expression to use when true.</param>
    /// <param name="expressionFalse">Expression to use when false.</param>
    public OperatorTernaryExpression(ExpressionDefinition expression, ExpressionDefinition expressionTrue, ExpressionDefinition expressionFalse)
    {
        _expression = expression;
        _expressionTrue = expressionTrue;
        _expressionFalse = expressionFalse;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _expression.GetDependencies(definition).Union(_expressionTrue.GetDependencies(definition)).Union(_expressionFalse.GetDependencies(definition));

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        throw new NotImplementedException();
    }
}
