using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Unary operator expression
/// </summary>
public class OperatorUnaryExpression : ExpressionDefinition
{
    /// <summary>
    /// Operator
    /// </summary>
    public enum Operator
#pragma warning disable 1591
    {
        Plus,
        Minus,

        //Not,
        Tilde
    }
#pragma warning restore 1591

    private readonly ExpressionDefinition _expression;
    private readonly Operator _operator;

    /// <summary>
    /// Create new instance of <see cref="OperatorUnaryExpression"/>
    /// </summary>
    /// <param name="expression">Expression</param>
    /// <param name="op">Operator</param>
    public OperatorUnaryExpression(ExpressionDefinition expression, Operator op)
    {
        _expression = expression;
        _operator = op;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _expression.GetDependencies(definition);

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        return _operator switch
        {
            Operator.Plus => new OperatorUnaryPlusExpressionInstance(_expression.GetInstance()),
            Operator.Minus => new OperatorUnaryMinusExpressionInstance(_expression.GetInstance()),
            Operator.Tilde => new OperatorUnaryTildeExpressionInstance(_expression.GetInstance()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private record OperatorUnaryPlusExpressionInstance(ExpressionInstance Expression) : ExpressionInstance
    {
        public override object Evaluate(StructureInstance structure, Stream stream)
        {
            object value = Expression.Evaluate(structure, stream) ?? throw new NullReferenceException("Expr value null");

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

    private record OperatorUnaryMinusExpressionInstance(ExpressionInstance Expression) : ExpressionInstance
    {
        public override object Evaluate(StructureInstance structure, Stream stream)
        {
            object value = Expression.Evaluate(structure, stream) ?? throw new NullReferenceException("Expr value null");

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

    private record OperatorUnaryTildeExpressionInstance(ExpressionInstance Expression) : ExpressionInstance
    {
        public override object Evaluate(StructureInstance structure, Stream stream)
        {
            object value = Expression.Evaluate(structure, stream) ?? throw new NullReferenceException("Expr value null");

            return value switch
            {
                long longValue => ~longValue,
                ulong ulongValue => ~ulongValue,
                int intValue => ~intValue,
                uint uintValue => ~uintValue,
                short shortValue => ~shortValue,
                ushort ushortValue => ~ushortValue,
                sbyte sbyteValue => ~sbyteValue,
                byte byteValue => ~byteValue,
                _ => new Exception($"No suitable types found for operator, was type {value.GetType().FullName}")
            };
        }
    }

    /// <summary>
    /// Get operator
    /// </summary>
    /// <param name="op">String representation</param>
    /// <returns>Enum value</returns>
    public static Operator GetOperator(string op)
    {
        return op switch
        {
            "+" => Operator.Plus,
            "-" => Operator.Minus,
            "~" => Operator.Tilde,
            _ => throw new ArgumentOutOfRangeException(nameof(op))
        };
    }
}
