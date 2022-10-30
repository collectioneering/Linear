using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Elements;

/// <summary>
/// Element defining lambda.
/// </summary>
public class LambdaElement : Element
{
    private readonly string _name;
    private readonly ExpressionDefinition _expression;

    /// <summary>
    /// Create new instance of <see cref="ValueElement"/>
    /// </summary>
    /// <param name="name">Name of element</param>
    /// <param name="expression">Value definition</param>
    public LambdaElement(string name, ExpressionDefinition expression)
    {
        _name = name;
        _expression = expression;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) => _expression.GetDependencies(definition);

    /// <inheritdoc />
    public override ElementInitializer GetInitializer()
    {
        return new LambdaElementInitializer(_expression.GetInstance(), _name);
    }

    private record LambdaElementInitializer(ExpressionInstance Expression, string Name) : ElementInitializer
    {
        public override void Initialize(StructureEvaluationContext context, Stream stream)
        {
            context.Structure.SetMember(Name, Expression);
        }

        public override void Initialize(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            context.Structure.SetMember(Name, Expression);
        }
    }
}
