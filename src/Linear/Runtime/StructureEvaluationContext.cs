using System.Collections.Generic;

namespace Linear.Runtime;

/// <summary>
/// Structure evaluation context.
/// </summary>
/// <param name="Structure">Structure.</param>
/// <param name="LambdaReplacements">Active lambda replacements.</param>
public readonly record struct StructureEvaluationContext(StructureInstance Structure, IReadOnlyDictionary<string, object>? LambdaReplacements = null);
