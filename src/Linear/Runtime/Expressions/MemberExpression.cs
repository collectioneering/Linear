using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions;

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
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return definition.Members.Where(x => x.Item1 == _name).Select(x => x.Item2);
    }

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() => new MemberExpressionInstance(_name);

    private record MemberExpressionInstance(string Name) : ExpressionInstance
    {
        public override object Evaluate(StructureInstance structure, Stream stream)
        {
            return structure[Name];
        }
    }
}
