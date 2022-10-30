namespace Linear.Runtime.Expressions;

/// <summary>
/// Delegate type for evaluation expression.
/// </summary>
/// <param name="context">Structure evaluation context.</param>
/// <param name="args">Arguments.</param>
public delegate object? MethodCallDelegate(StructureEvaluationContext context, params object?[] args);
