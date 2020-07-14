using System.Collections.Generic;

namespace Linear.Runtime
{
    /// <summary>
    /// Parsed body of structure
    /// </summary>
    public class StructureInstance
    {

        /// <summary>
        /// Absolute offset of structure
        /// </summary>
        public int AbsoluteOffset { get; }

        internal readonly Dictionary<string, object> _members = new Dictionary<string, object>();


            /// <summary>
            /// Create new instance of <see cref="StructureInstance"/>
            /// </summary>
            /// <param name="absoluteOffset"></param>
            public StructureInstance(int absoluteOffset)
        {
            AbsoluteOffset = absoluteOffset;
        }


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
