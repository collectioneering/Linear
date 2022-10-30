using System;
using System.IO;
using Linear.Utility;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorDualAddExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
{
    public override object Evaluate(StructureEvaluationContext context, Stream stream)
    {
        object? left = Left.Evaluate(context, stream);
        object? right = Right.Evaluate(context, stream);
        return EvaluateInternal(left, right);
    }

    public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
    {
        object? left = Left.Evaluate(context, span);
        object? right = Right.Evaluate(context, span);
        return EvaluateInternal(left, right);
    }

    private static object EvaluateInternal(object? left, object? right)
    {
        if (left is string strLeft) return strLeft + right;
        if (right is string strRight) return left + strRight;

        if (left == null) throw new NullReferenceException("LHS null");
        if (right == null) throw new NullReferenceException("RHS null");

        if (left is double doubleLeft) return doubleLeft + CastUtil.CastDouble(right);
        if (right is double doubleRight) return CastUtil.CastDouble(left) + doubleRight;

        if (left is float floatLeft) return floatLeft + CastUtil.CastFloat(right);
        if (right is float floatRight) return CastUtil.CastFloat(left) + floatRight;

        if (left is long longLeft) return longLeft + CastUtil.CastLong(right);
        if (right is long longRight) return CastUtil.CastLong(left) + longRight;

        if (left is ulong ulongLeft) return ulongLeft + CastUtil.CastULong(right);
        if (right is ulong ulongRight) return CastUtil.CastULong(left) + ulongRight;

        if (left is int intLeft) return intLeft + CastUtil.CastInt(right);
        if (right is int intRight) return CastUtil.CastInt(left) + intRight;

        if (left is uint uintLeft) return uintLeft + CastUtil.CastUInt(right);
        if (right is uint uintRight) return CastUtil.CastUInt(left) + uintRight;

        if (left is short shortLeft) return shortLeft + CastUtil.CastShort(right);
        if (right is short shortRight) return CastUtil.CastShort(left) + shortRight;

        if (left is ushort ushortLeft) return ushortLeft + CastUtil.CastUShort(right);
        if (right is ushort ushortRight) return CastUtil.CastUShort(left) + ushortRight;

        if (left is sbyte sbyteLeft) return sbyteLeft + CastUtil.CastSByte(right);
        if (right is sbyte sbyteRight) return CastUtil.CastSByte(left) + sbyteRight;

        if (left is byte byteLeft) return byteLeft + CastUtil.CastByte(right);
        if (right is byte byteRight) return CastUtil.CastByte(left) + byteRight;
        throw new Exception("No suitable types found for operator");
    }
}
