using System;
using System.IO;
using Linear.Utility;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorDualUrshiftExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
{
    public override object Evaluate(StructureEvaluationContext context, Stream stream)
    {
        object left = Left.Evaluate(context, stream) ?? throw new NullReferenceException("LHS null");
        object right = Right.Evaluate(context, stream) ?? throw new NullReferenceException("RHS null");
        return EvaluateInternal(left, right);
    }

    public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
    {
        object left = Left.Evaluate(context, memory) ?? throw new NullReferenceException("LHS null");
        object right = Right.Evaluate(context, memory) ?? throw new NullReferenceException("RHS null");
        return EvaluateInternal(left, right);
    }

    public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
    {
        object left = Left.Evaluate(context, span) ?? throw new NullReferenceException("LHS null");
        object right = Right.Evaluate(context, span) ?? throw new NullReferenceException("RHS null");
        return EvaluateInternal(left, right);
    }

    private static object EvaluateInternal(object left, object right)
    {
        if (left is long longLeft) return longLeft >>> CastUtil.CastInt(right);

        if (left is ulong ulongLeft) return ulongLeft >>> CastUtil.CastInt(right);

        if (left is int intLeft) return intLeft >>> CastUtil.CastInt(right);

        if (left is uint uintLeft) return uintLeft >>> CastUtil.CastInt(right);

        if (left is short shortLeft) return shortLeft >>> CastUtil.CastInt(right);

        if (left is ushort ushortLeft) return ushortLeft >>> CastUtil.CastInt(right);

        if (left is sbyte sbyteLeft) return sbyteLeft >>> CastUtil.CastInt(right);

        if (left is byte byteLeft) return byteLeft >>> CastUtil.CastInt(right);

        throw new Exception("No suitable types found for operator");
    }
}
