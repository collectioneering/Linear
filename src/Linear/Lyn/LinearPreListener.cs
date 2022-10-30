using System.Collections.Generic;
using Antlr4.Runtime.Tree;

namespace Linear.Lyn;

/// <summary>
/// ANTLR listener implementation
/// </summary>
internal class LinearPreListener : LinearBaseListener
{
    private readonly List<string> _structureNames;
    internal bool Fail { get; private set; }

    public LinearPreListener()
    {
        _structureNames = new List<string>();
    }

    /// <summary>
    /// Get parsed structures
    /// </summary>
    /// <returns>Structures</returns>
    public IReadOnlyList<string> GetStructureNames() => _structureNames;

    public override void ExitStruct(LinearParser.StructContext context)
    {
        _structureNames.Add(context.IDENTIFIER().GetText());
    }

    public override void VisitErrorNode(IErrorNode node)
    {
        Fail = true;
    }
}
