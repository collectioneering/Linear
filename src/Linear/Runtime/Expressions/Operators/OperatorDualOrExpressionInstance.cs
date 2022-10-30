using System;
using System.IO;
using Linear.Utility;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorDualOrExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
{
    public override object Evaluate(StructureInstance structure, Stream stream)
    {
        object left = Left.Evaluate(structure, stream) ?? throw new NullReferenceException("LHS null");
        object right = Right.Evaluate(structure, stream) ?? throw new NullReferenceException("RHS null");
        return EvaluateInternal(left, right);
    }

    public override object Evaluate(StructureInstance structure, ReadOnlySpan<byte> span)
    {
        object left = Left.Evaluate(structure, span) ?? throw new NullReferenceException("LHS null");
        object right = Right.Evaluate(structure, span) ?? throw new NullReferenceException("RHS null");
        return EvaluateInternal(left, right);
    }

    private static object EvaluateInternal(object left, object right)
    {
        if (left is long longLeft) return longLeft | CastUtil.CastLong(right);
        if (right is long longRight) return CastUtil.CastLong(left) | longRight;

        if (left is ulong ulongLeft) return ulongLeft | CastUtil.CastULong(right);
        if (right is ulong ulongRight) return CastUtil.CastULong(left) | ulongRight;

        if (left is int intLeft) return intLeft | CastUtil.CastInt(right);
        if (right is int intRight) return CastUtil.CastInt(left) | intRight;

        if (left is uint uintLeft) return uintLeft | CastUtil.CastUInt(right);
        if (right is uint uintRight) return CastUtil.CastUInt(left) | uintRight;

        if (left is short shortLeft) return shortLeft | CastUtil.CastShort(right);
        if (right is short shortRight) return CastUtil.CastShort(left) | shortRight;

        if (left is ushort ushortLeft) return ushortLeft | CastUtil.CastUShort(right);
        if (right is ushort ushortRight) return CastUtil.CastUShort(left) | ushortRight;

        if (left is sbyte sbyteLeft) return sbyteLeft | CastUtil.CastSByte(right);
        if (right is sbyte sbyteRight) return CastUtil.CastSByte(left) | sbyteRight;

        if (left is byte byteLeft) return byteLeft | CastUtil.CastByte(right);
        if (right is byte byteRight) return CastUtil.CastByte(left) | byteRight;
        throw new Exception("No suitable types found for operator");
    }
}
