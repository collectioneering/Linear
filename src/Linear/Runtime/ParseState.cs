namespace Linear.Runtime;

/// <summary>
/// Represents parse state of structure.
/// </summary>
/// <param name="StructureName">Name of current structure.</param>
/// <param name="Offset">Offset in stream.</param>
/// <param name="Parent">Parent object.</param>
/// <param name="Length">Length of structure.</param>
/// <param name="Index">Array index.</param>
public readonly record struct ParseState(string StructureName, long Offset = 0, StructureInstance? Parent = null, long Length = 0, int Index = 0);
