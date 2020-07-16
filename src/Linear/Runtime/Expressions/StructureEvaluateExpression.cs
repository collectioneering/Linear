using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions
{
    /// <summary>
    /// Index expression
    /// </summary>
    public class StructureEvaluateExpression<T> : ExpressionDefinition
    {
        /// <summary>
        /// Delegate type for evaluation expression
        /// </summary>
        /// <param name="instance">Structure instance</param>
        public delegate T StructureEvaluateDelegate(StructureInstance instance);

        private readonly StructureEvaluateDelegate _delegate;

        /// <summary>
        /// Create new instance of <see cref="StructureEvaluateExpression{T}"/>
        /// </summary>
        /// <param name="evaluateDelegate"></param>
        public StructureEvaluateExpression(StructureEvaluateDelegate evaluateDelegate)
        {
            _delegate = evaluateDelegate;
        }


        /// <inheritdoc />
        public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
            Enumerable.Empty<Element>();

        /// <inheritdoc />
        public override Func<StructureInstance, Stream, byte[], object?> GetDelegate() =>
            (instance, stream, tempBuffer) => _delegate(instance);
    }
}
