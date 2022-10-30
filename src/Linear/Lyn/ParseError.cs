namespace Linear.Lyn;

/// <summary>
/// Marks a parse error.
/// </summary>
/// <param name="Location">Source location.</param>
/// <param name="Error">Error text.</param>
public record ParseError(SourceLocation Location, string Error)
{
    /// <summary>
    /// Gets formatted text of error.
    /// </summary>
    /// <returns>Error text.</returns>
    public string GetFormattedText() => $"{Location.FilenameHint}({Location.Line},{Location.Column}): {Error}";
}
