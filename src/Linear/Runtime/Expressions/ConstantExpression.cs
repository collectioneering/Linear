using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Constant expression.
/// </summary>
public abstract class ConstantExpression : ExpressionDefinition
{
    /// <inheritdoc />
    public sealed override IEnumerable<Element> GetDependencies(StructureDefinition definition) => Enumerable.Empty<Element>();
}

/// <summary>
/// Constant expression
/// </summary>
public class ConstantExpression<T> : ConstantExpression
{
    private readonly T _value;

    /// <summary>
    /// Create new instance of <see cref="ConstantExpression{T}"/>
    /// </summary>
    /// <param name="value">Value</param>
    public ConstantExpression(T value)
    {
        _value = value;
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() => new ConstantExpressionInstance(_value);

    private record ConstantExpressionInstance(T Value) : ExpressionInstance
    {
        public override object? Evaluate(StructureEvaluationContext context, Stream stream)
        {
            return Value;
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            return Value;
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            return Value;
        }
    }
}

/// <inheritdoc />
public abstract class ConstantNumberExpression : ExpressionDefinition
{
    internal abstract record ConstantExpressionInstance : ExpressionInstance
    {
#if NET7_0_OR_GREATER
        /// <summary>
        /// Casts this numeric expression to the target numeric type.
        /// </summary>
        /// <typeparam name="TResult">Target numeric type</typeparam>
        /// <returns>Casted value.</returns>
        public abstract TResult Cast<TResult>() where TResult : System.Numerics.INumber<TResult>;
#endif
    }
}

/// <inheritdoc />
public class ConstantNumberExpression<T> : ConstantNumberExpression
#if NET7_0_OR_GREATER
    where T : System.Numerics.INumber<T>
#else
    where T : unmanaged
#endif
{
    private readonly T _value;

    /// <inheritdoc />
    public ConstantNumberExpression(T value)
    {
        _value = value;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) => Enumerable.Empty<Element>();

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() => new ConstantExpressionTInstance(_value);

    internal record ConstantExpressionTInstance(T Value) : ConstantExpressionInstance
    {
#if NET7_0_OR_GREATER
        /// <summary>
        /// Casts this numeric expression to the target numeric type.
        /// </summary>
        /// <typeparam name="TResult">Target numeric type</typeparam>
        /// <returns>Casted value.</returns>
        public override TResult Cast<TResult>()
        {
            // TODO
            throw new NotImplementedException();
        }
#endif

        public override object Evaluate(StructureEvaluationContext context, Stream stream)
        {
            return Value;
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            return Value;
        }

        public override object Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            return Value;
        }
    }
}
