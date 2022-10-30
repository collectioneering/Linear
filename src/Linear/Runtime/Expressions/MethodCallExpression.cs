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
        public override object? Evaluate(StructureInstance structure, Stream stream)
        {
            return Delegate(ArgsCompact.Select(arg => arg.Evaluate(structure, stream)).ToArray());
        }

        public override object? Evaluate(StructureInstance structure, ReadOnlySpan<byte> span)
        {
            List<object?> args = new();
            foreach (var arg in ArgsCompact)
            {
                args.Add(arg.Evaluate(structure, span));
            }
            return Delegate(args);
        }
    }
}
