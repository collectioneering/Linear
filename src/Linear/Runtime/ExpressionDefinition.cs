using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime
{
    /// <summary>
    /// Definition of expression
    /// </summary>
    public abstract class ExpressionDefinition
    {
        /// <summary>
        /// Expression type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Create new instance of <see cref="ExpressionDefinition"/>
        /// </summary>
        /// <param name="type">Expression type</param>
        protected ExpressionDefinition(Type type)
        {
            Type = type;
        }
        /// <summary>
        /// Determine dependencies on other members in structure
        /// </summary>
        /// <param name="definition">Structure to use</param>
        /// <returns>Dependencies</returns>
        /// <remarks>Does not resolve references to parent</remarks>
        public abstract List<ExpressionDefinition> GetDependencies(StructureDefinition definition);

        /// <summary>
        /// Get delegate to parse expression
        /// </summary>
        /// <returns>Delegate</returns>
        public abstract Func<StructureInstance, Stream, byte[], object> GetDelegate();
    }
}
