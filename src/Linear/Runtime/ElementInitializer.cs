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
    public abstract void Initialize(StructureEvaluationContext context, Stream stream);

    /// <summary>
    /// Initializes element.
    /// </summary>
    /// <param name="context">Structure evaluation context.</param>
    /// <param name="span">Span.</param>
    public abstract void Initialize(StructureEvaluationContext context, ReadOnlySpan<byte> span);
}
