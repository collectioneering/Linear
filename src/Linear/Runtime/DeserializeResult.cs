namespace Linear.Runtime;

/// <summary>
/// Deserialized result.
/// </summary>
/// <param name="Value">Deserialized value.</param>
/// <param name="Length">Deserialized length.</param>
public readonly record struct DeserializeResult(object Value, long Length);
