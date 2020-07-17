using System;
using System.Collections.Generic;
using System.IO;

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
        public override Action<StructureInstance, Stream, byte[]> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object?> expressionDelegate = _expression.GetDelegate();
            return (instance, stream, tempBuffer) =>
            {
                instance.Length = LinearCommon.CastLong(expressionDelegate(instance, stream, tempBuffer));
            };
        }
    }
}
