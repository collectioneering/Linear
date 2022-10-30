namespace Linear;

/// <summary>
/// Marks a source location.
/// </summary>
/// <param name="FilenameHint">Filename hint.</param>
/// <param name="Line">Line number.</param>
/// <param name="Column">Column.</param>
public readonly record struct SourceLocation(string? FilenameHint, int Line, int Column);
