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
        public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
        {
            IEnumerable<Element> deps = _startExpression.GetDependencies(definition);
            if (_endExpression != null) deps = deps.Union(_endExpression.GetDependencies(definition));
            if (_lengthExpression != null) deps = deps.Union(_lengthExpression.GetDependencies(definition));
            return deps;
        }

        /// <inheritdoc />
        public override Func<StructureInstance, Stream, byte[], object?> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object?> startDelegate = _startExpression.GetDelegate();
            if (_endExpression != null)
            {
                Func<StructureInstance, Stream, byte[], object?> endDelegate = _endExpression.GetDelegate();
                return (instance, stream, tempBuffer) =>
                {
                    long start = LinearCommon.CastLong(startDelegate(instance, stream, tempBuffer));
                    long end = LinearCommon.CastLong(endDelegate(instance, stream, tempBuffer));
                    return (start, end - start);
                };
            }

            Func<StructureInstance, Stream, byte[], object?> lengthDelegate = _lengthExpression!.GetDelegate();
            return (instance, stream, tempBuffer) =>
            {
                long start = LinearCommon.CastLong(startDelegate(instance, stream, tempBuffer));
                long length = LinearCommon.CastLong(lengthDelegate(instance, stream, tempBuffer));
                return (start, length);
            };
        }
    }
}
