using System.Collections.Generic;
using Linear.Runtime;

namespace Linear;

/// <summary>
/// Structure evaluation context.
/// </summary>
/// <param name="Structure">Structure.</param>
/// <param name="LambdaReplacements">Active lambda replacements.</param>
public readonly record struct StructureEvaluationContext(StructureInstance Structure, IReadOnlyDictionary<string, object>? LambdaReplacements = null);
