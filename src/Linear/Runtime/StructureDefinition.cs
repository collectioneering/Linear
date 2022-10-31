using System;
using System.Collections.Generic;
using System.Linq;
using Linear.Runtime.Elements;

namespace Linear.Runtime;

/// <summary>
/// Definition of structure
/// </summary>
public class StructureDefinition
{
    /// <summary>
    /// Name of structure
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Default length of structure
    /// </summary>
    public int? DefaultLength { get; }

    /// <summary>
    /// Members not sorted for dependency
    /// </summary>
    public List<StructureDefinitionMember> Members { get; }

    /// <summary>
    /// Create new instance of <see cref="StructureDefinition"/>
    /// </summary>
    /// <param name="name">Name of structure</param>
    /// <param name="defaultLength">Default length of structure</param>
    public StructureDefinition(string name, int? defaultLength)
    {
        Name = name;
        DefaultLength = defaultLength;
        Members = new List<StructureDefinitionMember>();
    }

    /// <summary>
    /// Build structure layout
    /// </summary>
    /// <returns>Structure</returns>
    public Structure Build()
    {
        List<StructureMember> members = new();
        var sub = new List<StructureDefinitionMember>(Members);
        // Build members after organizing by dependencies
        while (sub.Count > 0)
        {
            // Discards require that all following elements are gated behind them, so limit processing until discard comes first
            // If first remaining is a discard, remove that
            var v0 = sub[0];
            if (v0.Element is DiscardElement)
            {
                members.Add(new StructureMember(v0.Name, v0.Element.GetInitializer()));
                sub.RemoveAt(0);
                continue;
            }
            int discardIndex = sub.FindIndex(v => v.Element is DiscardElement);
            // Remove everything before first remaining discard
            int removed = 0;
            for (int i = 0; i < sub.Count && (discardIndex == -1 || i < discardIndex); i++)
            {
                var e = sub[i];
                bool remove = !e.Element.GetDependencies(this).Intersect(sub.Select(x => x.Element)).Any();
                if (remove)
                {
                    members.Add(new StructureMember(e.Name, e.Element.GetInitializer()));
                    sub.RemoveAt(i);
                    removed++;
                    discardIndex--;
                }
            }
            if (removed == 0) throw new Exception("Failed to reduce dependencies");
        }

        return new Structure(DefaultLength, members);
    }
}

/// <summary>
/// Represents an entry in a structure definition.
/// </summary>
/// <param name="Name">Entry name.</param>
/// <param name="Element">Element.</param>
public readonly record struct StructureDefinitionMember(string? Name, Element Element);
