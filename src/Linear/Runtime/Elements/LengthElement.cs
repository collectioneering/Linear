using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Linear.Utility;

namespace Linear.Runtime.Elements;

/// <summary>
/// Element sets length
/// </summary>
public class LengthElement : Element
{
    private readonly ExpressionDefinition _expression;

    /// <summary>
    /// Initializes an instance of <see cref="LengthElement"/>.
    /// </summary>
    /// <param name="expression">Value definition.</param>
    public LengthElement(ExpressionDefinition expression)
    {
        _expression = expression;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) => _expression.GetDependencies(definition);

    /// <inheritdoc />
    public override ElementInitializer GetInitializer()
    {
        return new LengthElementInitializer(_expression.GetInstance());
    }

    private record LengthElementInitializer(ExpressionInstance Expression) : ElementInitializer
    {
        public override ElementInitializeResult Initialize(StructureEvaluationContext context, Stream stream)
        {
            context.Structure.Length = CastUtil.CastLong(Expression.Evaluate(context, stream));
            return ElementInitializeResult.Default;
        }

        public override async ValueTask<ElementInitializeResult> InitializeAsync(StructureEvaluationContext context, Stream stream, CancellationToken cancellationToken = default)
        {
            context.Structure.Length = CastUtil.CastLong(await Expression.EvaluateAsync(context, stream, cancellationToken));
            return ElementInitializeResult.Default;
        }

        public override ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlyMemory<byte> memory)
        {
            context.Structure.Length = CastUtil.CastLong(Expression.Evaluate(context, memory));
            return ElementInitializeResult.Default;
        }

        public override async ValueTask<ElementInitializeResult> InitializeAsync(StructureEvaluationContext context, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken = default)
        {
            context.Structure.Length = CastUtil.CastLong(await Expression.EvaluateAsync(context, memory, cancellationToken));
            return ElementInitializeResult.Default;
        }

        public override ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            context.Structure.Length = CastUtil.CastLong(Expression.Evaluate(context, span));
            return ElementInitializeResult.Default;
        }
    }
}
