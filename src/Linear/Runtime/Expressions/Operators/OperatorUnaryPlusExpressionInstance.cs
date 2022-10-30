using System;
using System.IO;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorUnaryPlusExpressionInstance(ExpressionInstance Expression) : ExpressionInstance
{
    public override object Evaluate(StructureInstance structure, Stream stream)
    {
        object value = Expression.Evaluate(structure, stream) ?? throw new NullReferenceException("Expr value null");
        return EvaluateInternal(value);
    }

    public override object Evaluate(StructureInstance structure, ReadOnlySpan<byte> span)
    {
        object value = Expression.Evaluate(structure, span) ?? throw new NullReferenceException("Expr value null");
        return EvaluateInternal(value);
    }

    private static object EvaluateInternal(object value)
    {
        return value switch
        {
            double doubleValue => doubleValue,
            float floatValue => floatValue,
            long longValue => longValue,
            ulong ulongValue => ulongValue,
            int intValue => intValue,
            uint uintValue => uintValue,
            short shortValue => shortValue,
            ushort ushortValue => ushortValue,
            sbyte sbyteValue => sbyteValue,
            byte byteValue => byteValue,
            _ => new Exception($"No suitable types found for operator, was type {value.GetType().FullName}")
        };
    }
}
