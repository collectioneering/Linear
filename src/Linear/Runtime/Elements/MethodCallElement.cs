using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Elements
{
    /// <summary>
    /// Element calling method
    /// </summary>
    public class MethodCallElement : Element
    {
        private readonly ExpressionDefinition _expression;

        /// <summary>
        /// Create new instance of <see cref="MethodCallElement"/>
        /// </summary>
        /// <param name="expression">Value definition</param>
        public MethodCallElement(ExpressionDefinition expression)
        {
            _expression = expression;
        }

        /// <inheritdoc />
        public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
            _expression.GetDependencies(definition);

        /// <inheritdoc />
        public override Action<StructureInstance, Stream, byte[]> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object?> expressionDelegate = _expression.GetDelegate();
            return (instance, stream, tempBuffer) => { expressionDelegate(instance, stream, tempBuffer); };
        }
    }
}
