using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Elements;

/// <summary>
/// Element calling method
/// </summary>
public class MethodCallElement : Element
{
    private readonly ExpressionDefinition _expression;

    /// <summary>
    /// Create new instance of <see cref="MethodCallElement"/>
    /// </summary>
    /// <param name="expression">Value definition</param>
    public MethodCallElement(ExpressionDefinition expression)
    {
        _expression = expression;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) => _expression.GetDependencies(definition);

    /// <inheritdoc />
    public override ElementInitializer GetInitializer()
    {
        return new MethodCallElementInitializer(_expression.GetInstance());
    }

    private record MethodCallElementInitializer(ExpressionInstance Expression) : ElementInitializer
    {
        public override ElementInitializeResult Initialize(StructureEvaluationContext context, Stream stream)
        {
            Expression.Evaluate(context, stream);
            return ElementInitializeResult.Default;
        }

        public override ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            Expression.Evaluate(context, memory);
            return ElementInitializeResult.Default;
        }

        public override ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            Expression.Evaluate(context, span);
            return ElementInitializeResult.Default;
        }
    }
}
