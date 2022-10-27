using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Dual operator expression
/// </summary>
public class OperatorDualExpression : ExpressionDefinition
{
    /// <summary>
    /// Operator
    /// </summary>
    public enum Operator
#pragma warning disable 1591
    {
        Add,
        Sub,
        Div,
        Mult,
        Mod,
        And,
        Or,
        Xor
    }
#pragma warning restore 1591

    private readonly ExpressionDefinition _left;
    private readonly ExpressionDefinition _right;
    private readonly Operator _operator;

    /// <summary>
    /// Create new instance of <see cref="OperatorDualExpression"/>
    /// </summary>
    /// <param name="left">Left expression</param>
    /// <param name="right">Right expression</param>
    /// <param name="op">Operator</param>
    public OperatorDualExpression(ExpressionDefinition left, ExpressionDefinition right, Operator op)
    {
        _left = left;
        _right = right;
        _operator = op;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _left.GetDependencies(definition).Union(_right.GetDependencies(definition));

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        return _operator switch
        {
            Operator.Add => new OperatorDualAddExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            Operator.Sub => new OperatorDualSubExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            Operator.Div => new OperatorDualDivExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            Operator.Mult => new OperatorDualMultExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            Operator.Mod => new OperatorDualModExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            Operator.And => new OperatorDualAndExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            Operator.Or => new OperatorDualOrExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            Operator.Xor => new OperatorDualXorExpressionInstance(_left.GetInstance(), _right.GetInstance()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private record OperatorDualAddExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
    {
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            object? left = Left.Evaluate(structure, stream);
            object? right = Right.Evaluate(structure, stream);

            if (left is string strLeft) return strLeft + right;
            if (right is string strRight) return left + strRight;

            if (left == null) throw new NullReferenceException("LHS null");
            if (right == null) throw new NullReferenceException("RHS null");

            if (left is double doubleLeft) return doubleLeft + LinearCommon.CastDouble(right);
            if (right is double doubleRight) return LinearCommon.CastDouble(left) + doubleRight;

            if (left is float floatLeft) return floatLeft + LinearCommon.CastFloat(right);
            if (right is float floatRight) return LinearCommon.CastFloat(left) + floatRight;

            if (left is long longLeft) return longLeft + LinearCommon.CastLong(right);
            if (right is long longRight) return LinearCommon.CastLong(left) + longRight;

            if (left is ulong ulongLeft) return ulongLeft + LinearCommon.CastULong(right);
            if (right is ulong ulongRight) return LinearCommon.CastULong(left) + ulongRight;

            if (left is int intLeft) return intLeft + LinearCommon.CastInt(right);
            if (right is int intRight) return LinearCommon.CastInt(left) + intRight;

            if (left is uint uintLeft) return uintLeft + LinearCommon.CastUInt(right);
            if (right is uint uintRight) return LinearCommon.CastUInt(left) + uintRight;

            if (left is short shortLeft) return shortLeft + LinearCommon.CastShort(right);
            if (right is short shortRight) return LinearCommon.CastShort(left) + shortRight;

            if (left is ushort ushortLeft) return ushortLeft + LinearCommon.CastUShort(right);
            if (right is ushort ushortRight) return LinearCommon.CastUShort(left) + ushortRight;

            if (left is sbyte sbyteLeft) return sbyteLeft + LinearCommon.CastSByte(right);
            if (right is sbyte sbyteRight) return LinearCommon.CastSByte(left) + sbyteRight;

            if (left is byte byteLeft) return byteLeft + LinearCommon.CastByte(right);
            if (right is byte byteRight) return LinearCommon.CastByte(left) + byteRight;
            return new Exception("No suitable types found for operator");
        }
    }

    private record OperatorDualSubExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
    {
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            object left = Left.Evaluate(structure, stream) ?? throw new NullReferenceException("LHS null");
            object right = Right.Evaluate(structure, stream) ?? throw new NullReferenceException("RHS null");

            if (left is double doubleLeft) return doubleLeft - LinearCommon.CastDouble(right);
            if (right is double doubleRight) return LinearCommon.CastDouble(left) - doubleRight;

            if (left is float floatLeft) return floatLeft - LinearCommon.CastFloat(right);
            if (right is float floatRight) return LinearCommon.CastFloat(left) - floatRight;

            if (left is long longLeft) return longLeft - LinearCommon.CastLong(right);
            if (right is long longRight) return LinearCommon.CastLong(left) - longRight;

            if (left is ulong ulongLeft) return ulongLeft - LinearCommon.CastULong(right);
            if (right is ulong ulongRight) return LinearCommon.CastULong(left) - ulongRight;

            if (left is int intLeft) return intLeft - LinearCommon.CastInt(right);
            if (right is int intRight) return LinearCommon.CastInt(left) - intRight;

            if (left is uint uintLeft) return uintLeft - LinearCommon.CastUInt(right);
            if (right is uint uintRight) return LinearCommon.CastUInt(left) - uintRight;

            if (left is short shortLeft) return shortLeft - LinearCommon.CastShort(right);
            if (right is short shortRight) return LinearCommon.CastShort(left) - shortRight;

            if (left is ushort ushortLeft) return ushortLeft - LinearCommon.CastUShort(right);
            if (right is ushort ushortRight) return LinearCommon.CastUShort(left) - ushortRight;

            if (left is sbyte sbyteLeft) return sbyteLeft - LinearCommon.CastSByte(right);
            if (right is sbyte sbyteRight) return LinearCommon.CastSByte(left) - sbyteRight;

            if (left is byte byteLeft) return byteLeft - LinearCommon.CastByte(right);
            if (right is byte byteRight) return LinearCommon.CastByte(left) - byteRight;
            return new Exception("No suitable types found for operator");
        }
    }

    private record OperatorDualMultExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
    {
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            object left = Left.Evaluate(structure, stream) ?? throw new NullReferenceException("LHS null");
            object right = Right.Evaluate(structure, stream) ?? throw new NullReferenceException("RHS null");

            if (left is double doubleLeft) return doubleLeft * LinearCommon.CastDouble(right);
            if (right is double doubleRight) return LinearCommon.CastDouble(left) * doubleRight;

            if (left is float floatLeft) return floatLeft * LinearCommon.CastFloat(right);
            if (right is float floatRight) return LinearCommon.CastFloat(left) * floatRight;

            if (left is long longLeft) return longLeft * LinearCommon.CastLong(right);
            if (right is long longRight) return LinearCommon.CastLong(left) * longRight;

            if (left is ulong ulongLeft) return ulongLeft * LinearCommon.CastULong(right);
            if (right is ulong ulongRight) return LinearCommon.CastULong(left) * ulongRight;

            if (left is int intLeft) return intLeft * LinearCommon.CastInt(right);
            if (right is int intRight) return LinearCommon.CastInt(left) * intRight;

            if (left is uint uintLeft) return uintLeft * LinearCommon.CastUInt(right);
            if (right is uint uintRight) return LinearCommon.CastUInt(left) * uintRight;

            if (left is short shortLeft) return shortLeft * LinearCommon.CastShort(right);
            if (right is short shortRight) return LinearCommon.CastShort(left) * shortRight;

            if (left is ushort ushortLeft) return ushortLeft * LinearCommon.CastUShort(right);
            if (right is ushort ushortRight) return LinearCommon.CastUShort(left) * ushortRight;

            if (left is sbyte sbyteLeft) return sbyteLeft * LinearCommon.CastSByte(right);
            if (right is sbyte sbyteRight) return LinearCommon.CastSByte(left) * sbyteRight;

            if (left is byte byteLeft) return byteLeft * LinearCommon.CastByte(right);
            if (right is byte byteRight) return LinearCommon.CastByte(left) * byteRight;
            return new Exception("No suitable types found for operator");
        }
    }

    private record OperatorDualDivExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
    {
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            object left = Left.Evaluate(structure, stream) ?? throw new NullReferenceException("LHS null");
            object right = Right.Evaluate(structure, stream) ?? throw new NullReferenceException("RHS null");

            if (left is double doubleLeft) return doubleLeft / LinearCommon.CastDouble(right);
            if (right is double doubleRight) return LinearCommon.CastDouble(left) / doubleRight;

            if (left is float floatLeft) return floatLeft / LinearCommon.CastFloat(right);
            if (right is float floatRight) return LinearCommon.CastFloat(left) / floatRight;

            if (left is long longLeft) return longLeft / LinearCommon.CastLong(right);
            if (right is long longRight) return LinearCommon.CastLong(left) / longRight;

            if (left is ulong ulongLeft) return ulongLeft / LinearCommon.CastULong(right);
            if (right is ulong ulongRight) return LinearCommon.CastULong(left) / ulongRight;

            if (left is int intLeft) return intLeft / LinearCommon.CastInt(right);
            if (right is int intRight) return LinearCommon.CastInt(left) / intRight;

            if (left is uint uintLeft) return uintLeft / LinearCommon.CastUInt(right);
            if (right is uint uintRight) return LinearCommon.CastUInt(left) / uintRight;

            if (left is short shortLeft) return shortLeft / LinearCommon.CastShort(right);
            if (right is short shortRight) return LinearCommon.CastShort(left) / shortRight;

            if (left is ushort ushortLeft) return ushortLeft / LinearCommon.CastUShort(right);
            if (right is ushort ushortRight) return LinearCommon.CastUShort(left) / ushortRight;

            if (left is sbyte sbyteLeft) return sbyteLeft / LinearCommon.CastSByte(right);
            if (right is sbyte sbyteRight) return LinearCommon.CastSByte(left) / sbyteRight;

            if (left is byte byteLeft) return byteLeft / LinearCommon.CastByte(right);
            if (right is byte byteRight) return LinearCommon.CastByte(left) / byteRight;
            return new Exception("No suitable types found for operator");
        }
    }

    private record OperatorDualModExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
    {
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            object left = Left.Evaluate(structure, stream) ?? throw new NullReferenceException("LHS null");
            object right = Right.Evaluate(structure, stream) ?? throw new NullReferenceException("RHS null");

            if (left is double doubleLeft) return doubleLeft % LinearCommon.CastDouble(right);
            if (right is double doubleRight) return LinearCommon.CastDouble(left) % doubleRight;

            if (left is float floatLeft) return floatLeft % LinearCommon.CastFloat(right);
            if (right is float floatRight) return LinearCommon.CastFloat(left) % floatRight;

            if (left is long longLeft) return longLeft % LinearCommon.CastLong(right);
            if (right is long longRight) return LinearCommon.CastLong(left) % longRight;

            if (left is ulong ulongLeft) return ulongLeft % LinearCommon.CastULong(right);
            if (right is ulong ulongRight) return LinearCommon.CastULong(left) % ulongRight;

            if (left is int intLeft) return intLeft % LinearCommon.CastInt(right);
            if (right is int intRight) return LinearCommon.CastInt(left) % intRight;

            if (left is uint uintLeft) return uintLeft % LinearCommon.CastUInt(right);
            if (right is uint uintRight) return LinearCommon.CastUInt(left) % uintRight;

            if (left is short shortLeft) return shortLeft % LinearCommon.CastShort(right);
            if (right is short shortRight) return LinearCommon.CastShort(left) % shortRight;

            if (left is ushort ushortLeft) return ushortLeft % LinearCommon.CastUShort(right);
            if (right is ushort ushortRight) return LinearCommon.CastUShort(left) % ushortRight;

            if (left is sbyte sbyteLeft) return sbyteLeft % LinearCommon.CastSByte(right);
            if (right is sbyte sbyteRight) return LinearCommon.CastSByte(left) % sbyteRight;

            if (left is byte byteLeft) return byteLeft % LinearCommon.CastByte(right);
            if (right is byte byteRight) return LinearCommon.CastByte(left) % byteRight;
            return new Exception("No suitable types found for operator");
        }
    }

    private record OperatorDualAndExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
    {
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            object left = Left.Evaluate(structure, stream) ?? throw new NullReferenceException("LHS null");
            object right = Right.Evaluate(structure, stream) ?? throw new NullReferenceException("RHS null");

            if (left is long longLeft) return longLeft & LinearCommon.CastLong(right);
            if (right is long longRight) return LinearCommon.CastLong(left) & longRight;

            if (left is ulong ulongLeft) return ulongLeft & LinearCommon.CastULong(right);
            if (right is ulong ulongRight) return LinearCommon.CastULong(left) & ulongRight;

            if (left is int intLeft) return intLeft & LinearCommon.CastInt(right);
            if (right is int intRight) return LinearCommon.CastInt(left) & intRight;

            if (left is uint uintLeft) return uintLeft & LinearCommon.CastUInt(right);
            if (right is uint uintRight) return LinearCommon.CastUInt(left) & uintRight;

            if (left is short shortLeft) return shortLeft & LinearCommon.CastShort(right);
            if (right is short shortRight) return LinearCommon.CastShort(left) & shortRight;

            if (left is ushort ushortLeft) return ushortLeft & LinearCommon.CastUShort(right);
            if (right is ushort ushortRight) return LinearCommon.CastUShort(left) & ushortRight;

            if (left is sbyte sbyteLeft) return sbyteLeft & LinearCommon.CastSByte(right);
            if (right is sbyte sbyteRight) return LinearCommon.CastSByte(left) & sbyteRight;

            if (left is byte byteLeft) return byteLeft & LinearCommon.CastByte(right);
            if (right is byte byteRight) return LinearCommon.CastByte(left) & byteRight;
            return new Exception("No suitable types found for operator");
        }
    }

    private record OperatorDualOrExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
    {
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            object left = Left.Evaluate(structure, stream) ?? throw new NullReferenceException("LHS null");
            object right = Right.Evaluate(structure, stream) ?? throw new NullReferenceException("RHS null");

            if (left is long longLeft) return longLeft | LinearCommon.CastLong(right);
            if (right is long longRight) return LinearCommon.CastLong(left) | longRight;

            if (left is ulong ulongLeft) return ulongLeft | LinearCommon.CastULong(right);
            if (right is ulong ulongRight) return LinearCommon.CastULong(left) | ulongRight;

            if (left is int intLeft) return intLeft | LinearCommon.CastInt(right);
            if (right is int intRight) return LinearCommon.CastInt(left) | intRight;

            if (left is uint uintLeft) return uintLeft | LinearCommon.CastUInt(right);
            if (right is uint uintRight) return LinearCommon.CastUInt(left) | uintRight;

            if (left is short shortLeft) return shortLeft | LinearCommon.CastShort(right);
            if (right is short shortRight) return LinearCommon.CastShort(left) | shortRight;

            if (left is ushort ushortLeft) return ushortLeft | LinearCommon.CastUShort(right);
            if (right is ushort ushortRight) return LinearCommon.CastUShort(left) | ushortRight;

            if (left is sbyte sbyteLeft) return sbyteLeft | LinearCommon.CastSByte(right);
            if (right is sbyte sbyteRight) return LinearCommon.CastSByte(left) | sbyteRight;

            if (left is byte byteLeft) return byteLeft | LinearCommon.CastByte(right);
            if (right is byte byteRight) return LinearCommon.CastByte(left) | byteRight;
            return new Exception("No suitable types found for operator");
        }
    }

    private record OperatorDualXorExpressionInstance(ExpressionInstance Left, ExpressionInstance Right) : ExpressionInstance
    {
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            object left = Left.Evaluate(structure, stream) ?? throw new NullReferenceException("LHS null");
            object right = Right.Evaluate(structure, stream) ?? throw new NullReferenceException("RHS null");

            if (left is long longLeft) return longLeft ^ LinearCommon.CastLong(right);
            if (right is long longRight) return LinearCommon.CastLong(left) ^ longRight;

            if (left is ulong ulongLeft) return ulongLeft ^ LinearCommon.CastULong(right);
            if (right is ulong ulongRight) return LinearCommon.CastULong(left) ^ ulongRight;

            if (left is int intLeft) return intLeft ^ LinearCommon.CastInt(right);
            if (right is int intRight) return LinearCommon.CastInt(left) ^ intRight;

            if (left is uint uintLeft) return uintLeft ^ LinearCommon.CastUInt(right);
            if (right is uint uintRight) return LinearCommon.CastUInt(left) ^ uintRight;

            if (left is short shortLeft) return shortLeft ^ LinearCommon.CastShort(right);
            if (right is short shortRight) return LinearCommon.CastShort(left) ^ shortRight;

            if (left is ushort ushortLeft) return ushortLeft ^ LinearCommon.CastUShort(right);
            if (right is ushort ushortRight) return LinearCommon.CastUShort(left) ^ ushortRight;

            if (left is sbyte sbyteLeft) return sbyteLeft ^ LinearCommon.CastSByte(right);
            if (right is sbyte sbyteRight) return LinearCommon.CastSByte(left) ^ sbyteRight;

            if (left is byte byteLeft) return byteLeft ^ LinearCommon.CastByte(right);
            if (right is byte byteRight) return LinearCommon.CastByte(left) ^ byteRight;
            return new Exception("No suitable types found for operator");
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
            "+" => Operator.Add,
            "-" => Operator.Sub,
            "*" => Operator.Mult,
            "/" => Operator.Div,
            "%" => Operator.Mod,
            "&" => Operator.And,
            "|" => Operator.Or,
            "^" => Operator.Xor,
            _ => throw new ArgumentOutOfRangeException(nameof(op))
        };
    }
}
