using System;
using System.Collections.Generic;

namespace Linear.Runtime.Elements
{
    /// <summary>
    /// Element defining value
    /// </summary>
    public class ValueElement : Element
    {
        private readonly string _name;
        private readonly ExpressionDefinition _expression;

        /// <summary>
        /// Create new instance of <see cref="ValueElement"/>
        /// </summary>
        /// <param name="name">Name of element</param>
        /// <param name="expression">Value definition</param>
        public ValueElement(string name, ExpressionDefinition expression)
        {
            _name = name;
            _expression = expression;
        }


        /// <inheritdoc />
        public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
            _expression.GetDependencies(definition);

        /// <inheritdoc />
        public override ElementInitDelegate GetDelegate()
        {
            DeserializerDelegate expressionDelegate = _expression.GetDelegate();
            return (instance, stream) =>
            {
                object expression = expressionDelegate(instance, stream) ?? throw new NullReferenceException();
                instance.SetMember(_name, expression);
            };
        }
    }
}
