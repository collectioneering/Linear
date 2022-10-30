namespace Linear.Runtime;

/// <summary>
/// Represents a source paired with an offset.
/// </summary>
/// <param name="Source">Source.</param>
/// <param name="Offset">Offset.</param>
public record SourceWithOffset(object Source, object Offset);
