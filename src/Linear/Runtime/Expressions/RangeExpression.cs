using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions
{
    /// <summary>
    /// Range expression
    /// </summary>
    public class RangeExpression : ExpressionDefinition
    {
        private readonly ExpressionDefinition _startExpression;
        private readonly ExpressionDefinition? _endExpression;
        private readonly ExpressionDefinition? _lengthExpression;

        /// <summary>
        /// Create new instance of <see cref="RangeExpression"/>
        /// </summary>
        /// <param name="startExpression">Start offset expression</param>
        /// <param name="endExpression">End offset expression</param>
        /// <param name="lengthExpression">Length expression</param>
        public RangeExpression(ExpressionDefinition startExpression, ExpressionDefinition? endExpression,
            ExpressionDefinition? lengthExpression)
        {
            _startExpression = startExpression;
            _endExpression = endExpression;
            _lengthExpression = lengthExpression;
        }

        /// <inheritdoc />
        public override List<Element> GetDependencies(StructureDefinition definition)
        {
            IEnumerable<Element> deps = _startExpression.GetDependencies(definition);
            if (_endExpression != null) deps = deps.Union(_endExpression.GetDependencies(definition));
            if (_lengthExpression != null) deps = deps.Union(_lengthExpression.GetDependencies(definition));
            return deps.ToList();
        }

        /// <inheritdoc />
        public override Func<StructureInstance, Stream, byte[], object> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object> startDelegate = _startExpression.GetDelegate();
            if (_endExpression != null)
            {
                Func<StructureInstance, Stream, byte[], object>
                    endDelegate = _endExpression.GetDelegate();
                return (instance, stream, tempBuffer) =>
                {
                    object start = startDelegate(instance, stream, tempBuffer);
                    object end = endDelegate(instance, stream, tempBuffer);
                    if (!LinearUtil.TryCast(start, out long startValue))
                        throw new InvalidCastException(
                            $"Could not cast expression of type {start.GetType().FullName} to type {nameof(Int64)}");
                    if (!LinearUtil.TryCast(end, out long endValue))
                        throw new InvalidCastException(
                            $"Could not cast expression of type {end.GetType().FullName} to type {nameof(Int64)}");
                    return (startValue, endValue - startValue);
                };
            }

            Func<StructureInstance, Stream, byte[], object>
                lengthDelegate = _lengthExpression!.GetDelegate();
            return (instance, stream, tempBuffer) =>
            {
                object start = startDelegate(instance, stream, tempBuffer);
                object length = lengthDelegate(instance, stream, tempBuffer);
                if (!LinearUtil.TryCast(start, out long startValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {start.GetType().FullName} to type {nameof(Int64)}");
                if (!LinearUtil.TryCast(length, out long lengthValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {length.GetType().FullName} to type {nameof(Int64)}");
                return (startValue, lengthValue);
            };
        }
    }
}
