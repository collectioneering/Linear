using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime.Elements;

/// <summary>
/// Element sets length
/// </summary>
public class LengthElement : Element
{
    private readonly ExpressionDefinition _expression;

    /// <summary>
    /// Create new instance of <see cref="LengthElement"/>
    /// </summary>
    /// <param name="expression">Value definition</param>
    public LengthElement(ExpressionDefinition expression)
    {
        _expression = expression;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _expression.GetDependencies(definition);

    /// <inheritdoc />
    public override ElementInitializer GetInitializer()
    {
        return new LengthElementInitializer(_expression.GetInstance());
    }

    private record LengthElementInitializer(ExpressionInstance Expression) : ElementInitializer
    {
        public override void Initialize(StructureInstance structure, Stream stream)
        {
            structure.Length = LinearCommon.CastLong(Expression.Evaluate(structure, stream));
        }
    }
}
