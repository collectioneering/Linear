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
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) => _expression.GetDependencies(definition);

    /// <inheritdoc />
    public override ElementInitializer GetInitializer()
    {
        return new ValueElementInitializer(_expression.GetInstance(), _name);
    }

    private record ValueElementInitializer(ExpressionInstance Expression, string Name) : ElementInitializer
    {
        public override ElementInitializeResult Initialize(StructureEvaluationContext context, Stream stream)
        {
            object expression = Expression.Evaluate(context, stream) ?? throw new NullReferenceException();
            context.Structure.SetMember(Name, expression);
            return ElementInitializeResult.Default;
        }

        public override ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            object expression = Expression.Evaluate(context, memory) ?? throw new NullReferenceException();
            context.Structure.SetMember(Name, expression);
            return ElementInitializeResult.Default;
        }

        public override ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            object expression = Expression.Evaluate(context, span) ?? throw new NullReferenceException();
            context.Structure.SetMember(Name, expression);
            return ElementInitializeResult.Default;
        }
    }
}
