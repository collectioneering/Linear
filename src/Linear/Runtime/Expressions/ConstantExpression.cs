using System.Collections.Generic;
using System.Linq;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Constant expression
/// </summary>
public class ConstantExpression<T> : ExpressionDefinition
{
    private readonly T _value;

    /// <summary>
    /// Create new instance of <see cref="ConstantExpression{T}"/>
    /// </summary>
    /// <param name="value">Value</param>
    public ConstantExpression(T value)
    {
        _value = value;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        Enumerable.Empty<Element>();

    /// <inheritdoc />
    public override DeserializerDelegate GetDelegate() => (_, _) => _value!;
}
