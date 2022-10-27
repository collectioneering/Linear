using System.Collections.Generic;
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
    public override DeserializerDelegate GetDelegate() =>
        (instance, _) => instance[_name];
}
