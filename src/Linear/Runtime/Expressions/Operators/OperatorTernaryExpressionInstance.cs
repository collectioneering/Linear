using System;
using System.IO;
using Linear.Utility;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorTernaryExpressionInstance(ExpressionInstance Expression, ExpressionInstance ExpressionTrue, ExpressionInstance ExpressionFalse) : ExpressionInstance
{
    public override object? Evaluate(StructureEvaluationContext context, Stream stream)
    {
        return CastUtil.CastBool(Expression.Evaluate(context, stream)) ? ExpressionTrue.Evaluate(context, stream) : ExpressionFalse.Evaluate(context, stream);
    }

    public override object? Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
    {
        return CastUtil.CastBool(Expression.Evaluate(context, memory)) ? ExpressionTrue.Evaluate(context, memory) : ExpressionFalse.Evaluate(context, memory);
    }

    public override object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
    {
        return CastUtil.CastBool(Expression.Evaluate(context, span)) ? ExpressionTrue.Evaluate(context, span) : ExpressionFalse.Evaluate(context, span);
    }
}
