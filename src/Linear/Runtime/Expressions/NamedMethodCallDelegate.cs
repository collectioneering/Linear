namespace Linear.Runtime.Expressions;

/// <summary>
/// Represents a named method call delegate.
/// </summary>
/// <param name="Name">Name.</param>
/// <param name="Delegate">Delegate.</param>
public readonly record struct NamedMethodCallDelegate(string Name, MethodCallDelegate Delegate);
