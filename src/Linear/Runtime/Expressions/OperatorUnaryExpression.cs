using System;
using System.Collections.Generic;

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
    public override DeserializerDelegate GetDelegate()
    {
        DeserializerDelegate delExpr = _expression.GetDelegate();

        return _operator switch
        {
            Operator.Plus => (instance, stream) =>
            {
                object value = delExpr(instance, stream) ??
                               throw new NullReferenceException("Expr value null");

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
            },
            Operator.Minus => (instance, stream) =>
            {
                object value = delExpr(instance, stream) ??
                               throw new NullReferenceException("Expr value null");

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
            },
            Operator.Tilde => (instance, stream) =>
            {
                object value = delExpr(instance, stream) ??
                               throw new NullReferenceException("Expr value null");

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
            },
            _ => throw new ArgumentOutOfRangeException()
        };
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
