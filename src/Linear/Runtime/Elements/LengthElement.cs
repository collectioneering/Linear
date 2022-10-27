using System.Collections.Generic;

namespace Linear.Runtime.Elements
{
    /// <summary>
    /// Element sets length
    /// </summary>
    public class LengthElement : Element
    {
        private readonly ExpressionDefinition _expression;

        /// <summary>
        /// Create new instance of <see cref="LengthElement"/>
        /// </summary>
        /// <param name="expression">Value definition</param>
        public LengthElement(ExpressionDefinition expression)
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
            return (instance, stream) => { instance.Length = LinearCommon.CastLong(expressionDelegate(instance, stream)); };
        }
    }
}
