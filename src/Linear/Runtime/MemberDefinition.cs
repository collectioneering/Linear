using System;

namespace Linear.Runtime
{
    /// <summary>
    /// Definition of structure member
    /// </summary>
    public class MemberDefinition
    {
        /// <summary>
        /// Member name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Member type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Member expression
        /// </summary>
        public ExpressionDefinition Expression { get; }

        /// <summary>
        /// Create new instance of <see cref="MemberDefinition"/>
        /// </summary>
        /// <param name="name">Member name</param>
        /// <param name="type">Member type</param>
        /// <param name="expression">Member expression</param>
        public MemberDefinition(string name, Type type, ExpressionDefinition expression)
        {
            Name = name;
            Type = type;
            Expression = expression;
        }
    }
}
