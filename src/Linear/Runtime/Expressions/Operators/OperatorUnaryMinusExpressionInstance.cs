using System;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorUnaryMinusExpressionInstance(ExpressionInstance Expression) : OperatorUnaryExpressionInstance(Expression)
{
    protected override object Evaluate(object? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Expr value null");
        }
        return value switch
        {
            double doubleValue => -doubleValue,
            float floatValue => -floatValue,
            long longValue => -longValue,
            int intValue => -intValue,
            uint uintValue => -uintValue,
            short shortValue => -shortValue,
            ushort ushortValue => -ushortValue,
            sbyte sbyteValue => -sbyteValue,
            byte byteValue => -byteValue,
            _ => new Exception($"No suitable types found for operator, was type {value.GetType().FullName}")
        };
    }
}
