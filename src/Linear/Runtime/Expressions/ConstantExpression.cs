using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Expressions
{
    /// <summary>
    /// Constant expression
    /// </summary>
    public class ConstantExpression<T> : ExpressionDefinition
    {
        private readonly T _value;

        /// <summary>
        /// Create new instance of <see cref="ConstantExpression"/>
        /// </summary>
        /// <param name="value">Value</param>
        public ConstantExpression(T value) : base(typeof(T))
        {
            _value = value;
        }

        /// <inheritdoc />
        public override List<ExpressionDefinition> GetDependencies(StructureDefinition definition) =>
            new List<ExpressionDefinition>();

        /// <inheritdoc />
        public override Func<StructureInstance, Stream, byte[], object> GetDelegate() => (a, b, c) => _value!;
    }
}
