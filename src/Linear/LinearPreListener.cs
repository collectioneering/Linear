using System.Collections.Generic;

namespace Linear
{
    /// <summary>
    /// ANTLR listener implementation
    /// </summary>
    internal class LinearPreListener : LinearBaseListener
    {
        private readonly List<string> _structureNames;

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
    }
}
