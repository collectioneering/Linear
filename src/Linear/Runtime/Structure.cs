using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime
{
    /// <summary>
    /// Structure layout
    /// </summary>
    public class Structure
    {
        private readonly List<(string name, Func<StructureInstance, Stream, object> method)> _members;

        /// <summary>
        /// Create new instance of <see cref="Structure"/>
        /// </summary>
        /// <param name="members"></param>
        public Structure(List<(string name, Func<StructureInstance, Stream, object> method)> members)
        {
            _members = members;
        }

        /// <summary>
        /// Parse structure from stream
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <returns>Parsed structure</returns>
        public StructureInstance Parse(Stream stream)
        {
            // TODO evaluate all
            return new StructureInstance(0);
        }
    }
}
