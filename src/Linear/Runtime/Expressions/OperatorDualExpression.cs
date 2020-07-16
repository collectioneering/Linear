using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions
{
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
        public override Func<StructureInstance, Stream, byte[], object?> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object?> delLeft = _left.GetDelegate();
            Func<StructureInstance, Stream, byte[], object?> delRight = _right.GetDelegate();

            return _operator switch
            {
                Operator.Add => (instance, stream, tempBuffer) =>
                {
                    object? left = delLeft(instance, stream, tempBuffer);
                    object? right = delRight(instance, stream, tempBuffer);

                    if (left is string strLeft) return strLeft + right;
                    if (right is string strRight) return left + strRight;

                    if (left == null) throw new NullReferenceException("LHS null");
                    if (right == null) throw new NullReferenceException("RHS null");

                    if (left is double doubleLeft) return doubleLeft + LinearUtil.CastDouble(right);
                    if (right is double doubleRight) return LinearUtil.CastDouble(left) + doubleRight;

                    if (left is float floatLeft) return floatLeft + LinearUtil.CastFloat(right);
                    if (right is float floatRight) return LinearUtil.CastFloat(left) + floatRight;

                    if (left is long longLeft) return longLeft + LinearUtil.CastLong(right);
                    if (right is long longRight) return LinearUtil.CastLong(left) + longRight;

                    if (left is ulong ulongLeft) return ulongLeft + LinearUtil.CastULong(right);
                    if (right is ulong ulongRight) return LinearUtil.CastULong(left) + ulongRight;

                    if (left is int intLeft) return intLeft + LinearUtil.CastInt(right);
                    if (right is int intRight) return LinearUtil.CastInt(left) + intRight;

                    if (left is uint uintLeft) return uintLeft + LinearUtil.CastUInt(right);
                    if (right is uint uintRight) return LinearUtil.CastUInt(left) + uintRight;

                    if (left is short shortLeft) return shortLeft + LinearUtil.CastShort(right);
                    if (right is short shortRight) return LinearUtil.CastShort(left) + shortRight;

                    if (left is ushort ushortLeft) return ushortLeft + LinearUtil.CastUShort(right);
                    if (right is ushort ushortRight) return LinearUtil.CastUShort(left) + ushortRight;

                    if (left is sbyte sbyteLeft) return sbyteLeft + LinearUtil.CastSByte(right);
                    if (right is sbyte sbyteRight) return LinearUtil.CastSByte(left) + sbyteRight;

                    if (left is byte byteLeft) return byteLeft + LinearUtil.CastByte(right);
                    if (right is byte byteRight) return LinearUtil.CastByte(left) + byteRight;
                    return new Exception("No suitable types found for operator");
                },
                Operator.Sub => (instance, stream, tempBuffer) =>
                {
                    object left = delLeft(instance, stream, tempBuffer) ??
                                  throw new NullReferenceException("LHS null");
                    object right = delRight(instance, stream, tempBuffer) ??
                                   throw new NullReferenceException("RHS null");

                    if (left is double doubleLeft) return doubleLeft - LinearUtil.CastDouble(right);
                    if (right is double doubleRight) return LinearUtil.CastDouble(left) - doubleRight;

                    if (left is float floatLeft) return floatLeft - LinearUtil.CastFloat(right);
                    if (right is float floatRight) return LinearUtil.CastFloat(left) - floatRight;

                    if (left is long longLeft) return longLeft - LinearUtil.CastLong(right);
                    if (right is long longRight) return LinearUtil.CastLong(left) - longRight;

                    if (left is ulong ulongLeft) return ulongLeft - LinearUtil.CastULong(right);
                    if (right is ulong ulongRight) return LinearUtil.CastULong(left) - ulongRight;

                    if (left is int intLeft) return intLeft - LinearUtil.CastInt(right);
                    if (right is int intRight) return LinearUtil.CastInt(left) - intRight;

                    if (left is uint uintLeft) return uintLeft - LinearUtil.CastUInt(right);
                    if (right is uint uintRight) return LinearUtil.CastUInt(left) - uintRight;

                    if (left is short shortLeft) return shortLeft - LinearUtil.CastShort(right);
                    if (right is short shortRight) return LinearUtil.CastShort(left) - shortRight;

                    if (left is ushort ushortLeft) return ushortLeft - LinearUtil.CastUShort(right);
                    if (right is ushort ushortRight) return LinearUtil.CastUShort(left) - ushortRight;

                    if (left is sbyte sbyteLeft) return sbyteLeft - LinearUtil.CastSByte(right);
                    if (right is sbyte sbyteRight) return LinearUtil.CastSByte(left) - sbyteRight;

                    if (left is byte byteLeft) return byteLeft - LinearUtil.CastByte(right);
                    if (right is byte byteRight) return LinearUtil.CastByte(left) - byteRight;
                    return new Exception("No suitable types found for operator");
                },
                Operator.Div => (instance, stream, tempBuffer) =>
                {
                    object left = delLeft(instance, stream, tempBuffer) ??
                                  throw new NullReferenceException("LHS null");
                    object right = delRight(instance, stream, tempBuffer) ??
                                   throw new NullReferenceException("RHS null");

                    if (left is double doubleLeft) return doubleLeft / LinearUtil.CastDouble(right);
                    if (right is double doubleRight) return LinearUtil.CastDouble(left) / doubleRight;

                    if (left is float floatLeft) return floatLeft / LinearUtil.CastFloat(right);
                    if (right is float floatRight) return LinearUtil.CastFloat(left) / floatRight;

                    if (left is long longLeft) return longLeft / LinearUtil.CastLong(right);
                    if (right is long longRight) return LinearUtil.CastLong(left) / longRight;

                    if (left is ulong ulongLeft) return ulongLeft / LinearUtil.CastULong(right);
                    if (right is ulong ulongRight) return LinearUtil.CastULong(left) / ulongRight;

                    if (left is int intLeft) return intLeft / LinearUtil.CastInt(right);
                    if (right is int intRight) return LinearUtil.CastInt(left) / intRight;

                    if (left is uint uintLeft) return uintLeft / LinearUtil.CastUInt(right);
                    if (right is uint uintRight) return LinearUtil.CastUInt(left) / uintRight;

                    if (left is short shortLeft) return shortLeft / LinearUtil.CastShort(right);
                    if (right is short shortRight) return LinearUtil.CastShort(left) / shortRight;

                    if (left is ushort ushortLeft) return ushortLeft / LinearUtil.CastUShort(right);
                    if (right is ushort ushortRight) return LinearUtil.CastUShort(left) / ushortRight;

                    if (left is sbyte sbyteLeft) return sbyteLeft / LinearUtil.CastSByte(right);
                    if (right is sbyte sbyteRight) return LinearUtil.CastSByte(left) / sbyteRight;

                    if (left is byte byteLeft) return byteLeft / LinearUtil.CastByte(right);
                    if (right is byte byteRight) return LinearUtil.CastByte(left) / byteRight;
                    return new Exception("No suitable types found for operator");
                },
                Operator.Mult => (instance, stream, tempBuffer) =>
                {
                    object left = delLeft(instance, stream, tempBuffer) ??
                                  throw new NullReferenceException("LHS null");
                    object right = delRight(instance, stream, tempBuffer) ??
                                   throw new NullReferenceException("RHS null");

                    if (left is double doubleLeft) return doubleLeft * LinearUtil.CastDouble(right);
                    if (right is double doubleRight) return LinearUtil.CastDouble(left) * doubleRight;

                    if (left is float floatLeft) return floatLeft * LinearUtil.CastFloat(right);
                    if (right is float floatRight) return LinearUtil.CastFloat(left) * floatRight;

                    if (left is long longLeft) return longLeft * LinearUtil.CastLong(right);
                    if (right is long longRight) return LinearUtil.CastLong(left) * longRight;

                    if (left is ulong ulongLeft) return ulongLeft * LinearUtil.CastULong(right);
                    if (right is ulong ulongRight) return LinearUtil.CastULong(left) * ulongRight;

                    if (left is int intLeft) return intLeft * LinearUtil.CastInt(right);
                    if (right is int intRight) return LinearUtil.CastInt(left) * intRight;

                    if (left is uint uintLeft) return uintLeft * LinearUtil.CastUInt(right);
                    if (right is uint uintRight) return LinearUtil.CastUInt(left) * uintRight;

                    if (left is short shortLeft) return shortLeft * LinearUtil.CastShort(right);
                    if (right is short shortRight) return LinearUtil.CastShort(left) * shortRight;

                    if (left is ushort ushortLeft) return ushortLeft * LinearUtil.CastUShort(right);
                    if (right is ushort ushortRight) return LinearUtil.CastUShort(left) * ushortRight;

                    if (left is sbyte sbyteLeft) return sbyteLeft * LinearUtil.CastSByte(right);
                    if (right is sbyte sbyteRight) return LinearUtil.CastSByte(left) * sbyteRight;

                    if (left is byte byteLeft) return byteLeft * LinearUtil.CastByte(right);
                    if (right is byte byteRight) return LinearUtil.CastByte(left) * byteRight;
                    return new Exception("No suitable types found for operator");
                },
                Operator.Mod => (instance, stream, tempBuffer) =>
                {
                    object left = delLeft(instance, stream, tempBuffer) ??
                                  throw new NullReferenceException("LHS null");
                    object right = delRight(instance, stream, tempBuffer) ??
                                   throw new NullReferenceException("RHS null");

                    if (left is double doubleLeft) return doubleLeft % LinearUtil.CastDouble(right);
                    if (right is double doubleRight) return LinearUtil.CastDouble(left) % doubleRight;

                    if (left is float floatLeft) return floatLeft % LinearUtil.CastFloat(right);
                    if (right is float floatRight) return LinearUtil.CastFloat(left) % floatRight;

                    if (left is long longLeft) return longLeft % LinearUtil.CastLong(right);
                    if (right is long longRight) return LinearUtil.CastLong(left) % longRight;

                    if (left is ulong ulongLeft) return ulongLeft % LinearUtil.CastULong(right);
                    if (right is ulong ulongRight) return LinearUtil.CastULong(left) % ulongRight;

                    if (left is int intLeft) return intLeft % LinearUtil.CastInt(right);
                    if (right is int intRight) return LinearUtil.CastInt(left) % intRight;

                    if (left is uint uintLeft) return uintLeft % LinearUtil.CastUInt(right);
                    if (right is uint uintRight) return LinearUtil.CastUInt(left) % uintRight;

                    if (left is short shortLeft) return shortLeft % LinearUtil.CastShort(right);
                    if (right is short shortRight) return LinearUtil.CastShort(left) % shortRight;

                    if (left is ushort ushortLeft) return ushortLeft % LinearUtil.CastUShort(right);
                    if (right is ushort ushortRight) return LinearUtil.CastUShort(left) % ushortRight;

                    if (left is sbyte sbyteLeft) return sbyteLeft % LinearUtil.CastSByte(right);
                    if (right is sbyte sbyteRight) return LinearUtil.CastSByte(left) % sbyteRight;

                    if (left is byte byteLeft) return byteLeft % LinearUtil.CastByte(right);
                    if (right is byte byteRight) return LinearUtil.CastByte(left) % byteRight;
                    return new Exception("No suitable types found for operator");
                },
                Operator.And => (instance, stream, tempBuffer) =>
                {
                    object left = delLeft(instance, stream, tempBuffer) ??
                                  throw new NullReferenceException("LHS null");
                    object right = delRight(instance, stream, tempBuffer) ??
                                   throw new NullReferenceException("RHS null");

                    if (left is long longLeft) return longLeft & LinearUtil.CastLong(right);
                    if (right is long longRight) return LinearUtil.CastLong(left) & longRight;

                    if (left is ulong ulongLeft) return ulongLeft & LinearUtil.CastULong(right);
                    if (right is ulong ulongRight) return LinearUtil.CastULong(left) & ulongRight;

                    if (left is int intLeft) return intLeft & LinearUtil.CastInt(right);
                    if (right is int intRight) return LinearUtil.CastInt(left) & intRight;

                    if (left is uint uintLeft) return uintLeft & LinearUtil.CastUInt(right);
                    if (right is uint uintRight) return LinearUtil.CastUInt(left) & uintRight;

                    if (left is short shortLeft) return shortLeft & LinearUtil.CastShort(right);
                    if (right is short shortRight) return LinearUtil.CastShort(left) & shortRight;

                    if (left is ushort ushortLeft) return ushortLeft & LinearUtil.CastUShort(right);
                    if (right is ushort ushortRight) return LinearUtil.CastUShort(left) & ushortRight;

                    if (left is sbyte sbyteLeft) return sbyteLeft & LinearUtil.CastSByte(right);
                    if (right is sbyte sbyteRight) return LinearUtil.CastSByte(left) & sbyteRight;

                    if (left is byte byteLeft) return byteLeft & LinearUtil.CastByte(right);
                    if (right is byte byteRight) return LinearUtil.CastByte(left) & byteRight;
                    return new Exception("No suitable types found for operator");
                },
                Operator.Or => (instance, stream, tempBuffer) =>
                {
                    object left = delLeft(instance, stream, tempBuffer) ??
                                  throw new NullReferenceException("LHS null");
                    object right = delRight(instance, stream, tempBuffer) ??
                                   throw new NullReferenceException("RHS null");

                    if (left is long longLeft) return longLeft | LinearUtil.CastLong(right);
                    if (right is long longRight) return LinearUtil.CastLong(left) | longRight;

                    if (left is ulong ulongLeft) return ulongLeft | LinearUtil.CastULong(right);
                    if (right is ulong ulongRight) return LinearUtil.CastULong(left) | ulongRight;

                    if (left is int intLeft) return intLeft | LinearUtil.CastInt(right);
                    if (right is int intRight) return LinearUtil.CastInt(left) | intRight;

                    if (left is uint uintLeft) return uintLeft | LinearUtil.CastUInt(right);
                    if (right is uint uintRight) return LinearUtil.CastUInt(left) | uintRight;

                    if (left is short shortLeft) return shortLeft | LinearUtil.CastShort(right);
                    if (right is short shortRight) return LinearUtil.CastShort(left) | shortRight;

                    if (left is ushort ushortLeft) return ushortLeft | LinearUtil.CastUShort(right);
                    if (right is ushort ushortRight) return LinearUtil.CastUShort(left) | ushortRight;

                    if (left is sbyte sbyteLeft) return sbyteLeft | LinearUtil.CastSByte(right);
                    if (right is sbyte sbyteRight) return LinearUtil.CastSByte(left) | sbyteRight;

                    if (left is byte byteLeft) return byteLeft | LinearUtil.CastByte(right);
                    if (right is byte byteRight) return LinearUtil.CastByte(left) | byteRight;
                    return new Exception("No suitable types found for operator");
                },
                Operator.Xor => (instance, stream, tempBuffer) =>
                {
                    object left = delLeft(instance, stream, tempBuffer) ??
                                  throw new NullReferenceException("LHS null");
                    object right = delRight(instance, stream, tempBuffer) ??
                                   throw new NullReferenceException("RHS null");

                    if (left is long longLeft) return longLeft ^ LinearUtil.CastLong(right);
                    if (right is long longRight) return LinearUtil.CastLong(left) ^ longRight;

                    if (left is ulong ulongLeft) return ulongLeft ^ LinearUtil.CastULong(right);
                    if (right is ulong ulongRight) return LinearUtil.CastULong(left) ^ ulongRight;

                    if (left is int intLeft) return intLeft ^ LinearUtil.CastInt(right);
                    if (right is int intRight) return LinearUtil.CastInt(left) ^ intRight;

                    if (left is uint uintLeft) return uintLeft ^ LinearUtil.CastUInt(right);
                    if (right is uint uintRight) return LinearUtil.CastUInt(left) ^ uintRight;

                    if (left is short shortLeft) return shortLeft ^ LinearUtil.CastShort(right);
                    if (right is short shortRight) return LinearUtil.CastShort(left) ^ shortRight;

                    if (left is ushort ushortLeft) return ushortLeft ^ LinearUtil.CastUShort(right);
                    if (right is ushort ushortRight) return LinearUtil.CastUShort(left) ^ ushortRight;

                    if (left is sbyte sbyteLeft) return sbyteLeft ^ LinearUtil.CastSByte(right);
                    if (right is sbyte sbyteRight) return LinearUtil.CastSByte(left) ^ sbyteRight;

                    if (left is byte byteLeft) return byteLeft ^ LinearUtil.CastByte(right);
                    if (right is byte byteRight) return LinearUtil.CastByte(left) ^ byteRight;
                    return new Exception("No suitable types found for operator");
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
}
