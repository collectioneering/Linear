using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Elements;

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
    public override ElementInitializer GetInitializer()
    {
        return new ValueElementInitializer(_expression.GetInstance(), _name);
    }

    private record ValueElementInitializer(ExpressionInstance Expression, string Name) : ElementInitializer
    {
        public override void Initialize(StructureInstance structure, Stream stream)
        {
            object expression = Expression.Deserialize(structure, stream) ?? throw new NullReferenceException();
            structure.SetMember(Name, expression);
        }
    }
}
