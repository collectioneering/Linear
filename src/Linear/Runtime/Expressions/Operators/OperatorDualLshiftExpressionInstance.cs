using System;
using Linear.Utility;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorDualLshiftExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : OperatorDualExpressionInstance(Left, Right)
{
    protected override object Evaluate(object? left, object? right)
    {
        if (left == null) throw new NullReferenceException("LHS null");
        if (right == null) throw new NullReferenceException("RHS null");

        if (left is long longLeft) return longLeft << CastUtil.CastInt(right);

        if (left is ulong ulongLeft) return ulongLeft << CastUtil.CastInt(right);

        if (left is int intLeft) return intLeft << CastUtil.CastInt(right);

        if (left is uint uintLeft) return uintLeft << CastUtil.CastInt(right);

        if (left is short shortLeft) return shortLeft << CastUtil.CastInt(right);

        if (left is ushort ushortLeft) return ushortLeft << CastUtil.CastInt(right);

        if (left is sbyte sbyteLeft) return sbyteLeft << CastUtil.CastInt(right);

        if (left is byte byteLeft) return byteLeft << CastUtil.CastInt(right);

        throw new Exception("No suitable types found for operator");
    }
}
