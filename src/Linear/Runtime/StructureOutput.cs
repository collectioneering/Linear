using System.Collections.Generic;

namespace Linear.Runtime;

/// <summary>
/// Represents output for a structure.
/// </summary>
/// <param name="Instance">Source instance.</param>
/// <param name="Name">Name.</param>
/// <param name="Format">Format.</param>
/// <param name="Parameters">Parameters.</param>
/// <param name="Range">Range.</param>
public record StructureOutput(StructureInstance Instance, string Name, string Format, IReadOnlyDictionary<string, object>? Parameters, LongRange Range);
