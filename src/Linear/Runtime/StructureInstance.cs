using System.Collections.Generic;

namespace Linear.Runtime
{
    /// <summary>
    /// Parsed body of structure
    /// </summary>
    public class StructureInstance
    {
        /// <summary>
        /// Registry this structure belongs to
        /// </summary>
        public StructureRegistry Registry;

        /// <summary>
        /// Absolute offset of structure
        /// </summary>
        public long AbsoluteOffset { get; }

        /// <summary>
        /// Parent object
        /// </summary>
        public StructureInstance? Parent;

        /// <summary>
        /// Length of structure
        /// </summary>
        /// <remarks>
        /// Only available in trailing-length pointer array
        /// </remarks>
        public int Length { get; }

        private readonly Dictionary<string, object> _members = new Dictionary<string, object>();


        /// <summary>
        /// Create new instance of <see cref="StructureInstance"/>
        /// </summary>
        /// <param name="registry">Registry this instance belongs to</param>
        /// <param name="parent">Parent object</param>
        /// <param name="absoluteOffset">Absolute offset of structure</param>
        /// <param name="length">Length of structure</param>
        public StructureInstance(StructureRegistry registry, StructureInstance? parent, long absoluteOffset,
            int length = 0)
        {
            Registry = registry;
            Parent = parent;
            AbsoluteOffset = absoluteOffset;
            Length = length;
        }

        internal void SetMember(string name, object value) => _members[name] = value;

        /// <summary>
        /// Get named member
        /// </summary>
        /// <param name="member">Member name</param>
        /// <exception cref="KeyNotFoundException">If key was not found in members</exception>
        public object this[string member] => _members[member];

        /// <summary>
        /// Get member cast to type
        /// </summary>
        /// <param name="member">Member name</param>
        /// <typeparam name="T">Target type</typeparam>
        /// <returns>Value</returns>
        /// <exception cref="KeyNotFoundException">If key was not found in members</exception>
        public T GetCasted<T>(string member) => (T)_members[member];

        /// <summary>
        /// Try to get member cast to type
        /// </summary>
        /// <param name="member">Member name</param>
        /// <param name="result">Value</param>
        /// <typeparam name="T">Target type</typeparam>
        /// <returns>True if cast succeeded</returns>
        /// <exception cref="KeyNotFoundException">If key was not found in members</exception>
        public bool TryGetCasted<T>(string member, out T result) => LinearUtil.TryCast(_members[member], out result);
    }
}
