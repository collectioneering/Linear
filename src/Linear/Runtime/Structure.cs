using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Structure layout
/// </summary>
public class Structure
{
    /// <summary>
    /// Default length of structure
    /// </summary>
    public int? DefaultLength { get; }

    private readonly List<StructureMember> _members;

    /// <summary>
    /// Create new instance of <see cref="Structure"/>
    /// </summary>
    /// <param name="defaultLength">Default length of structure</param>
    /// <param name="members"></param>
    public Structure(int? defaultLength, List<StructureMember> members)
    {
        _members = members;
        DefaultLength = defaultLength;
    }

    /// <summary>
    /// Parses structure from stream.
    /// </summary>
    /// <param name="registry">Structure registry.</param>
    /// <param name="stream">Stream to read from.</param>
    /// <param name="parseState">Current parse state.</param>
    /// <returns>Parsed structure.</returns>
    public StructureInstance Parse(IReadOnlyDictionary<string, Structure> registry, Stream stream, ParseState parseState)
    {
        StructureInstance instance = new(registry, parseState.Parent, parseState.Offset, parseState.Length ?? DefaultLength, parseState.Index);
        foreach (var member in _members)
        {
            member.Initializer.Initialize(new StructureEvaluationContext(instance), stream);
        }

        return instance;
    }

    /// <summary>
    /// Parses structure from span.
    /// </summary>
    /// <param name="registry">Structure registry.</param>
    /// <param name="span">Buffer to read from.</param>
    /// <param name="parseState">Current parse state.</param>
    /// <returns>Parsed structure.</returns>
    public StructureInstance Parse(IReadOnlyDictionary<string, Structure> registry, ReadOnlySpan<byte> span, ParseState parseState)
    {
        StructureInstance instance = new(registry, parseState.Parent, parseState.Offset, parseState.Length ?? DefaultLength, parseState.Index);
        foreach (var member in _members)
        {
            member.Initializer.Initialize(new StructureEvaluationContext(instance), span);
        }

        return instance;
    }

    /// <summary>
    /// Parses structure from span.
    /// </summary>
    /// <param name="registry">Structure registry.</param>
    /// <param name="memory">Buffer to read from.</param>
    /// <param name="parseState">Current parse state.</param>
    /// <returns>Parsed structure.</returns>
    public StructureInstance Parse(IReadOnlyDictionary<string, Structure> registry, ReadOnlyMemory<byte> memory, ParseState parseState)
    {
        StructureInstance instance = new(registry, parseState.Parent, parseState.Offset, parseState.Length ?? DefaultLength, parseState.Index);
        foreach (var member in _members)
        {
            member.Initializer.Initialize(new StructureEvaluationContext(instance), memory);
        }

        return instance;
    }
}
