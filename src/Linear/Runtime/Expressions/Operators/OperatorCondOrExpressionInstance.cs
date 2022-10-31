using System;
using System.IO;
using Linear.Utility;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorCondOrExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
{
    public override object Evaluate(StructureEvaluationContext context, Stream stream)
    {
        return CastUtil.CastBool(Left.Evaluate(context, stream)) || CastUtil.CastBool(Right.Evaluate(context, stream));
    }

    public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
    {
        return CastUtil.CastBool(Left.Evaluate(context, memory)) || CastUtil.CastBool(Right.Evaluate(context, memory));
    }

    public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
    {
        return CastUtil.CastBool(Left.Evaluate(context, span)) || CastUtil.CastBool(Right.Evaluate(context, span));
    }
}
