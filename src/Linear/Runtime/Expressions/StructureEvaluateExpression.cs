using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Structure evaluation expression
/// </summary>
public class StructureEvaluateExpression<T> : ExpressionDefinition
{
    /// <summary>
    /// Delegate type for evaluation expression
    /// </summary>
    /// <param name="instance">Structure instance</param>
    public delegate T StructureEvaluateDelegate(StructureInstance instance);

    private readonly StructureEvaluateDelegate _delegate;

    /// <summary>
    /// Create new instance of <see cref="StructureEvaluateExpression{T}"/>
    /// </summary>
    /// <param name="evaluateDelegate">Delegate</param>
    public StructureEvaluateExpression(StructureEvaluateDelegate evaluateDelegate)
    {
        _delegate = evaluateDelegate;
    }


    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        Enumerable.Empty<Element>();

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() => new StructureEvaluateExpressionInstance(_delegate);

    private record StructureEvaluateExpressionInstance(StructureEvaluateDelegate Delegate) : ExpressionInstance
    {
        public override object? Deserialize(StructureInstance structure, Stream stream) => Delegate(structure);
    }
}
