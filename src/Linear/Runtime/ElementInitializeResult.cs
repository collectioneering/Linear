namespace Linear.Runtime;

/// <summary>
/// Represents result of running a <see cref="ElementInitializer"/>.
/// </summary>
/// <param name="Discard">Discard further elements.</param>
public readonly record struct ElementInitializeResult(bool Discard)
{
    /// <summary>
    /// Default result.
    /// </summary>
    public static readonly ElementInitializeResult Default = new ElementInitializeResult(false);
}
