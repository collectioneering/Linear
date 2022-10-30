using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Expressions;

/// <summary>
/// Method call expression
/// </summary>
public class MethodCallExpression : ExpressionDefinition
{
    private readonly MethodCallDelegate _delegate;
    private readonly List<ExpressionDefinition> _args;

    /// <summary>
    /// Create new instance of <see cref="MethodCallExpression"/>
    /// </summary>
    /// <param name="callDelegate">Delegate</param>
    /// <param name="args">Arguments</param>
    public MethodCallExpression(MethodCallDelegate callDelegate, List<ExpressionDefinition> args)
    {
        _delegate = callDelegate;
        _args = args;
    }


    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition) =>
        _args.SelectMany(a => a.GetDependencies(definition));

    /// <inheritdoc />
    public override ExpressionInstance GetInstance()
    {
        List<ExpressionInstance> argsCompact = _args.Select(arg => arg.GetInstance()).ToList();
        return new MethodCallExpressionInstance(argsCompact, _delegate);
    }

    private record MethodCallExpressionInstance(List<ExpressionInstance> ArgsCompact, MethodCallDelegate Delegate) : ExpressionInstance
    {
        public override object? Evaluate(StructureEvaluationContext context, Stream stream)
        {
            List<object?> args = new();
            foreach (var arg in ArgsCompact)
            {
                args.Add(arg.Evaluate(context, stream));
            }
            return Delegate(context, args.ToArray());
        }

        public override object? Evaluate(StructureEvaluationContext context, ReadOnlySpan<byte> span)
        {
            List<object?> args = new();
            foreach (var arg in ArgsCompact)
            {
                args.Add(arg.Evaluate(context, span));
            }
            return Delegate(context, args.ToArray());
        }
    }
}
