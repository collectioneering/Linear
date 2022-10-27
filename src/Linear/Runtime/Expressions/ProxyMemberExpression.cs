using System;
using System.Collections.Generic;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Proxy member expression
/// </summary>
public class ProxyMemberExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _source;
    private readonly string _name;

    /// <summary>
    /// Create new instance of <see cref="ProxyMemberExpression"/>
    /// </summary>
    /// <param name="name">Member name</param>
    /// <param name="source">Source expression</param>
    public ProxyMemberExpression(string name, ExpressionDefinition source)
    {
        _name = name;
        _source = source;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return _source.GetDependencies(definition);
    }

    /// <inheritdoc />
    public override DeserializerDelegate GetDelegate()
    {
        DeserializerDelegate del = _source.GetDelegate();
        return (instance, stream) =>
        {
            object? val = del(instance, stream);
            if (!LinearCommon.TryCast(val, out StructureInstance i2))
                throw new InvalidCastException(
                    $"Could not cast object of type {val?.GetType().FullName} to {nameof(StructureInstance)}");
            return i2[_name];
        };
    }
}
