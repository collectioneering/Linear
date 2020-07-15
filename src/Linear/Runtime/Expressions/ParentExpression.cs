using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions
{
    /// <summary>
    /// Parent expression
    /// </summary>
    public class ParentExpression : ExpressionDefinition
    {
        /// <inheritdoc />
        public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
            Enumerable.Empty<Element>();

        /// <inheritdoc />
        public override Func<StructureInstance, Stream, byte[], object?> GetDelegate() =>
            (instance, stream, tempBuffer) => instance.Parent;
    }
}
