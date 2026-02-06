using Linear.Utility;

namespace Linear.Runtime.Expressions.Operators;

internal record OperatorCondOrExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : OperatorDualExpressionInstance(Left, Right)
{
    protected override object Evaluate(object? left, object? right)
    {
        return CastUtil.CastBool(left) || CastUtil.CastBool(right);
    }
}
