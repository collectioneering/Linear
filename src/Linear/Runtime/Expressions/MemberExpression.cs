using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Expressions
{
    /// <summary>
    /// Member expression
    /// </summary>
    public class MemberExpression : ExpressionDefinition
    {
        private readonly string _name;

        /// <summary>
        /// Create new instance of <see cref="MemberExpression"/>
        /// </summary>
        /// <param name="name">Member name</param>
        public MemberExpression(string name)
        {
            _name = name;
        }

        /// <inheritdoc />
        public override List<ExpressionDefinition> GetDependencies(StructureDefinition definition) =>
            new List<ExpressionDefinition>();

        /// <inheritdoc />
        public override Func<StructureInstance, Stream, byte[], object> GetDelegate() =>
            (instance, stream, tempBuffer) => instance[_name];
    }
}
