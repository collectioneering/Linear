using System.Collections.Generic;
using System.Linq;

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
        public long Length { get; }

        private readonly Dictionary<string, object> _members = new Dictionary<string, object>();

        private readonly List<(string name, string format, Dictionary<string, object>? parameters,
            (long offset, long length) range)> _outputs =
            new List<(string name, string format, Dictionary<string, object>? parameters,
                (long offset, long length) range)>();

        /// <summary>
        /// Create new instance of <see cref="StructureInstance"/>
        /// </summary>
        /// <param name="registry">Registry this instance belongs to</param>
        /// <param name="parent">Parent object</param>
        /// <param name="absoluteOffset">Absolute offset of structure</param>
        /// <param name="length">Length of structure</param>
        public StructureInstance(StructureRegistry registry, StructureInstance? parent, long absoluteOffset,
            long length = 0)
        {
            Registry = registry;
            Parent = parent;
            AbsoluteOffset = absoluteOffset;
            Length = length;
        }

        internal void SetMember(string name, object value) => _members[name] = value;

        internal void AddOutput(
            (string name, string format, Dictionary<string, object>? parameters, (long offset, long length) range)
                value) =>
            _outputs.Add(value);

        /// <summary>
        /// Get outputs
        /// </summary>
        /// <returns>Outputs</returns>
        /// <param name="recurse">Recurse into children</param>
        public List<(StructureInstance instance, string name, string format, Dictionary<string, object>? parameters,
            (long offset, long length) range)> GetOutputs(
            bool recurse = true)
        {
            var outputs =
                new List<(StructureInstance instance, string name, string format,
                    Dictionary<string, object>? parameters, (long offset, long length) range)>(
                    _outputs.Select(x => (this, x.name, x.format, x.parameters, x.range)));
            if (recurse)
            {
                // TODO recurse into data elements
            }

            return outputs;
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
