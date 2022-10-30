using System;
using System.Collections.Generic;
using System.Linq;
using Linear.Runtime.Expressions.Operators;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Dual operator expression
/// </summary>
public class OperatorDualExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _left;
    private readonly ExpressionDefinition _right;
    private readonly BinaryOperator _operator;

    /// <summary>
    /// Create new instance of <see cref="OperatorDualExpression"/>
    /// </summary>
    /// <param name="left">Left expression</param>
    /// <param name="right">Right expression</param>
    /// <param name="op">Operator</param>
    public OperatorDualExpression(ExpressionDefinition left, ExpressionDefinition right, BinaryOperator op)
    {
        _left = left;
        _right = right;
        _operator = op;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _left.GetDependencies(definition).Union(_right.GetDependencies(definition));

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        return _operator switch
        {
            BinaryOperator.Add => new OperatorDualAddExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            BinaryOperator.Sub => new OperatorDualSubExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            BinaryOperator.Div => new OperatorDualDivExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            BinaryOperator.Mult => new OperatorDualMultExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            BinaryOperator.Mod => new OperatorDualModExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            BinaryOperator.And => new OperatorDualAndExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            BinaryOperator.Or => new OperatorDualOrExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            BinaryOperator.Xor => new OperatorDualXorExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Get operator
    /// </summary>
    /// <param name="op">String representation</param>
    /// <returns>Enum value</returns>
    public static BinaryOperator GetOperator(string op)
    {
        return op switch
        {
            "+" => BinaryOperator.Add,
            "-" => BinaryOperator.Sub,
            "*" => BinaryOperator.Mult,
            "/" => BinaryOperator.Div,
            "%" => BinaryOperator.Mod,
            "&" => BinaryOperator.And,
            "|" => BinaryOperator.Or,
            "^" => BinaryOperator.Xor,
            _ => throw new ArgumentOutOfRangeException(nameof(op))
        };
    }
}
