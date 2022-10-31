using System;
using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Initializer for element.
/// </summary>
public abstract record ElementInitializer
{
    /// <summary>
    /// Initializes element.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="stream">Stream.</param>
    /// <returns>Initialization result.</returns>
    public abstract ElementInitializeResult Initialize(StructureEvaluationContext context, Stream stream);

    /// <summary>
    /// Initializes element.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="memory">Memory.</param>
    /// <returns>Initialization result.</returns>
    public abstract ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlyMemory<byte> memory);

    /// <summary>
    /// Initializes element.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="span">Span.</param>
    /// <returns>Initialization result.</returns>
    public abstract ElementInitializeResult Initialize(StructureEvaluationContext context, ReadOnlySpan<byte> span);
}
