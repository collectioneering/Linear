using System.Collections.Generic;
using System.Linq;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Range expression
/// </summary>
public class RangeExpression : ExpressionDefinition
{
    private readonly ExpressionDefinition _startExpression;
    private readonly ExpressionDefinition? _endExpression;
    private readonly ExpressionDefinition? _lengthExpression;

    /// <summary>
    /// Create new instance of <see cref="RangeExpression"/>
    /// </summary>
    /// <param name="startExpression">Start offset expression</param>
    /// <param name="endExpression">End offset expression</param>
    /// <param name="lengthExpression">Length expression</param>
    public RangeExpression(ExpressionDefinition startExpression, ExpressionDefinition? endExpression,
        ExpressionDefinition? lengthExpression)
    {
        _startExpression = startExpression;
        _endExpression = endExpression;
        _lengthExpression = lengthExpression;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        IEnumerable<Element> deps = _startExpression.GetDependencies(definition);
        if (_endExpression != null) deps = deps.Union(_endExpression.GetDependencies(definition));
        if (_lengthExpression != null) deps = deps.Union(_lengthExpression.GetDependencies(definition));
        return deps;
    }

    /// <inheritdoc />
    public override DeserializerDelegate GetDelegate()
    {
        DeserializerDelegate startDelegate = _startExpression.GetDelegate();
        if (_endExpression != null)
        {
            DeserializerDelegate endDelegate = _endExpression.GetDelegate();
            return (instance, stream) =>
            {
                long start = LinearCommon.CastLong(startDelegate(instance, stream));
                long end = LinearCommon.CastLong(endDelegate(instance, stream));
                return (start, end - start);
            };
        }

        DeserializerDelegate lengthDelegate = _lengthExpression!.GetDelegate();
        return (instance, stream) =>
        {
            long start = LinearCommon.CastLong(startDelegate(instance, stream));
            long length = LinearCommon.CastLong(lengthDelegate(instance, stream));
            return (start, length);
        };
    }
}
