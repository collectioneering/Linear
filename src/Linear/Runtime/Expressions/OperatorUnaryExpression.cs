using System;
using System.Collections.Generic;
using Linear.Runtime.Expressions.Operators;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Unary operator expression.
/// </summary>
public class OperatorUnaryExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _expression;
    private readonly UnaryOperator _operator;

    /// <summary>
    /// Initializes an instance of <see cref="OperatorUnaryExpression"/>.
    /// </summary>
    /// <param name="expression">Expression.</param>
    /// <param name="op">Operator.</param>
    public OperatorUnaryExpression(ExpressionDefinition expression, UnaryOperator op)
    {
        _expression = expression;
        _operator = op;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _expression.GetDependencies(definition);

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        return _operator switch
        {
            UnaryOperator.Plus => new OperatorUnaryPlusExpressionInstance(_expression.GetInstance()),
            UnaryOperator.Minus => new OperatorUnaryMinusExpressionInstance(_expression.GetInstance()),
            UnaryOperator.Not => throw new NotImplementedException(),
            UnaryOperator.Tilde => new OperatorUnaryTildeExpressionInstance(_expression.GetInstance()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Gets operator.
    /// </summary>
    /// <param name="op">String representation.</param>
    /// <returns>Enum value.</returns>
    public static UnaryOperator GetOperator(string op)
    {
        return op switch
        {
            "+" => UnaryOperator.Plus,
            "-" => UnaryOperator.Minus,
            "!" => UnaryOperator.Not,
            "~" => UnaryOperator.Tilde,
            _ => throw new ArgumentOutOfRangeException(nameof(op))
        };
    }
}
