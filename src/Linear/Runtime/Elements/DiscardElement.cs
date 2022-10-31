using System;
using System.Collections.Generic;
using System.IO;
using Linear.Utility;

namespace Linear.Runtime.Elements;

/// <summary>
/// Element that triggers stopping initialization of later elements.
/// </summary>
public class DiscardElement : Element
{
    private readonly ExpressionDefinition _expression;

    /// <summary>
    /// Initializes an instance of <see cref="MethodCallElement"/>.
    /// </summary>
    /// <param name="expression">Evaluation expression definition.</param>
    public DiscardElement(ExpressionDefinition expression)
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
            bool discard = CastUtil.CastBool(Expression.Evaluate(context, stream));
            return new ElementInitializeResult(discard);
        }

        public override ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            bool discard = CastUtil.CastBool(Expression.Evaluate(context, memory));
            return new ElementInitializeResult(discard);
        }

        public override ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            bool discard = CastUtil.CastBool(Expression.Evaluate(context, span));
            return new ElementInitializeResult(discard);
        }
    }
}
