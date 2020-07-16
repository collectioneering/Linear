using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions
{
    /// <summary>
    /// Member expression
    /// </summary>
    public class ProxyMemberExpression : ExpressionDefinition
    {
        private readonly ExpressionDefinition _source;
        private readonly string _name;

        /// <summary>
        /// Create new instance of <see cref="ProxyMemberExpression"/>
        /// </summary>
        /// <param name="name">Member name</param>
        /// <param name="source">Source expression</param>
        public ProxyMemberExpression(string name, ExpressionDefinition source)
        {
            _name = name;
            _source = source;
        }

        /// <inheritdoc />
        public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
        {
            return definition.Members.Where(x => x.Item1 == _name).Select(x => x.Item2)
                .Union(_source.GetDependencies(definition));
        }

        /// <inheritdoc />
        public override Func<StructureInstance, Stream, byte[], object?> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object?> del = _source.GetDelegate();
            return (instance, stream, tempBuffer) =>
            {
                object? val = del(instance, stream, tempBuffer);
                if (!LinearUtil.TryCast(val, out StructureInstance i2))
                    throw new InvalidCastException(
                        $"Could not cast object of type {val?.GetType().FullName} to {nameof(StructureInstance)}");
                return i2[_name];
            };
        }
    }
}
