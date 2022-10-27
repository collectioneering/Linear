using System.Collections.Generic;

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
        public override ElementInitDelegate GetDelegate()
        {
            DeserializerDelegate expressionDelegate = _expression.GetDelegate();
            return (instance, stream, tempBuffer) => { expressionDelegate(instance, stream, tempBuffer); };
        }
    }
}
