using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Member expression
/// </summary>
public class ArrayAccessExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _source;
    private readonly ExpressionDefinition _index;

    /// <summary>
    /// Create new instance of <see cref="ProxyMemberExpression"/>
    /// </summary>
    /// <param name="source">Source expression</param>
    /// <param name="index">Index expression</param>
    public ArrayAccessExpression(ExpressionDefinition source, ExpressionDefinition index)
    {
        _source = source;
        _index = index;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _source.GetDependencies(definition).Union(_index.GetDependencies(definition));

    /// <inheritdoc />
    public override ExpressionInstance GetInstance() => new ArrayAccessExpressionInstance(_source.GetInstance(), _index.GetInstance());

    private record ArrayAccessExpressionInstance(ExpressionInstance Source, ExpressionInstance Index) : ExpressionInstance
    {
        public override object? Deserialize(StructureInstance structure, Stream stream)
        {
            object? source = Source.Deserialize(structure, stream);
            object index = Index.Deserialize(structure, stream) ?? throw new NullReferenceException();
            if (!LinearCommon.TryCast(source, out Array sourceValue))
                throw new InvalidCastException(
                    $"Could not cast object of type {source?.GetType().FullName} to {nameof(Array)}");
            return sourceValue.GetValue(LinearCommon.CastInt(index));
        }
    }
}
