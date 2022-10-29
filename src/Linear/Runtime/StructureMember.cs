namespace Linear.Runtime;

/// <summary>
/// Represents a structure member.
/// </summary>
/// <param name="Name">Member name.</param>
/// <param name="Initializer">Initializer.</param>
public readonly record struct StructureMember(string? Name, ElementInitializer Initializer);
