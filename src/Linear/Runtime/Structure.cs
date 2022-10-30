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
    public int DefaultLength { get; }

    private readonly List<StructureMember> _members;

    /// <summary>
    /// Create new instance of <see cref="Structure"/>
    /// </summary>
    /// <param name="defaultLength">Default length of structure</param>
    /// <param name="members"></param>
    public Structure(int defaultLength, List<StructureMember> members)
    {
        _members = members;
        DefaultLength = defaultLength;
    }

    /// <summary>
    /// Parses structure from stream.
    /// </summary>
    /// <param name="store">Store.</param>
    /// <param name="stream">Stream to read from.</param>
    /// <param name="parseState">Current parse state.</param>
    /// <returns>Parsed structure.</returns>
    public StructureInstance Parse(FormatStore store, Stream stream, ParseState parseState)
    {
        return Parse(store._registry, stream, parseState);
    }

    /// <summary>
    /// Parses structure from stream.
    /// </summary>
    /// <param name="registry">Structure registry.</param>
    /// <param name="stream">Stream to read from.</param>
    /// <param name="parseState">Current parse state.</param>
    /// <returns>Parsed structure.</returns>
    public StructureInstance Parse(StructureRegistry registry, Stream stream, ParseState parseState)
    {
        StructureInstance instance = new(registry, parseState.Parent, parseState.Offset, parseState.Length == 0 ? DefaultLength : parseState.Length, parseState.Index);
        foreach (var member in _members)
        {
            member.Initializer.Initialize(instance, stream);
        }

        return instance;
    }
}
