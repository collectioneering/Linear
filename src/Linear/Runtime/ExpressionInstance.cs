using System;
using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Expression instance.
/// </summary>
public abstract record ExpressionInstance
{
    /// <summary>
    /// Evaluates expression.
    /// </summary>
    /// <param name="structure">Structure.</param>
    /// <param name="stream">Stream.</param>
    /// <returns>Result of evaluation.</returns>
    public abstract object? Evaluate(StructureInstance structure, Stream stream);

    /// <summary>
    /// Evaluates expression.
    /// </summary>
    /// <param name="structure">Structure.</param>
    /// <param name="span">Span.</param>
    /// <returns>Result of evaluation.</returns>
    public abstract object? Evaluate(StructureInstance structure, ReadOnlySpan<byte> span);
}
