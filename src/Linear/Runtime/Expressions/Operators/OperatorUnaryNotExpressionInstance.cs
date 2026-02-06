using System;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorUnaryNotExpressionInstance(ExpressionInstance Expression) : OperatorUnaryExpressionInstance(Expression)
{
    protected override object Evaluate(object? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Expr value null");
        }
        return value switch
        {
            bool boolValue => !boolValue,
            _ => new Exception($"No suitable types found for operator, was type {value.GetType().FullName}")
        };
    }
}
