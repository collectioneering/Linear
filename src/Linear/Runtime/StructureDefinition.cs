﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime
{
    /// <summary>
    /// Definition of structure
    /// </summary>
    public class StructureDefinition
    {
        /// <summary>
        /// Members not sorted for dependency
        /// </summary>
        public List<MemberDefinition> Members { get; }

        /// <summary>
        /// Create new instance of <see cref="StructureDefinition"/>
        /// </summary>
        public StructureDefinition()
        {
            Members = new List<MemberDefinition>();
        }

        /// <summary>
        /// Build structure layout
        /// </summary>
        /// <returns>Structure</returns>
        public Structure Build()
        {
            List<(string name, Func<StructureInstance, Stream, object> method)> members =
                new List<(string name, Func<StructureInstance, Stream, object> method)>();
            // TODO build members
            return new Structure(members);
        }
    }
}
