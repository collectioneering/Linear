﻿#if NET7_0_OR_GREATER
using System;
#endif
using System.Collections.Generic;
using System.Linq;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif

namespace Linear.Runtime
{
    /// <summary>
    /// Parsed body of structure
    /// </summary>
    public class StructureInstance
    {
        private int _u;

        /// <summary>
        /// Index of structure in array
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Registry this structure belongs to
        /// </summary>
        public StructureRegistry Registry { get; }

        /// <summary>
        /// Absolute offset of structure
        /// </summary>
        public long AbsoluteOffset { get; }

        /// <summary>
        /// Parent object
        /// </summary>
        public StructureInstance? Parent { get; }

        /// <summary>
        /// Length of structure
        /// </summary>
        /// <remarks>
        /// Only available in trailing-length pointer array
        /// </remarks>
        public long Length { get; set; }

        /// <summary>
        /// Get unique sequential identifier
        /// </summary>
        /// <returns>Unique identifier</returns>
        public int GetUniqueId() => Parent?.GetUniqueId() ?? _u++;

        private readonly Dictionary<string, object> _members = new();

        private readonly List<StructureOutput> _outputs = new();

        /// <summary>
        /// Create new instance of <see cref="StructureInstance"/>
        /// </summary>
        /// <param name="registry">Registry this instance belongs to</param>
        /// <param name="parent">Parent object</param>
        /// <param name="absoluteOffset">Absolute offset of structure</param>
        /// <param name="length">Length of structure</param>
        /// <param name="i">Structure index</param>
        public StructureInstance(StructureRegistry registry, StructureInstance? parent, long absoluteOffset, long length = 0, int i = 0)
        {
            Registry = registry;
            Parent = parent;
            AbsoluteOffset = absoluteOffset;
            Length = length;
            Index = i;
        }

        internal void SetMember(string name, object value) => _members[name] = value;

        internal void AddOutput(StructureOutput output) => _outputs.Add(output);

        /// <summary>
        /// Get outputs
        /// </summary>
        /// <returns>Outputs</returns>
        /// <param name="recurse">Recurse into children</param>
        public List<StructureOutput> GetOutputs(bool recurse = true)
        {
            return GetOutputsInternal(recurse).ToList();
        }

        private IEnumerable<StructureOutput> GetOutputsInternal(bool recurse = true)
        {
            IEnumerable<StructureOutput> outputs = _outputs;
            if (recurse)
            {
                foreach (StructureInstance instance in _members.Values.OfType<StructureInstance>())
                    outputs = outputs.Concat(instance.GetOutputsInternal(recurse));
                foreach (StructureInstance instance in _members.Values.OfType<IEnumerable<StructureInstance>>().SelectMany(x => x))
                    outputs = outputs.Concat(instance.GetOutputsInternal(recurse));
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
        public T GetValue<T>(string member) => (T)_members[member];

        /// <summary>
        /// Try to get member cast to type
        /// </summary>
        /// <param name="member">Member name</param>
        /// <param name="result">Value</param>
        /// <typeparam name="T">Target type</typeparam>
        /// <returns>True if cast succeeded</returns>
        /// <exception cref="KeyNotFoundException">If key was not found in members</exception>
        public bool TryGetValue<T>(string member, out T? result)
        {
            result = default!;
            if (_members.TryGetValue(member, out object? v) && v is T v2)
            {
                result = v2;
                return true;
            }
            return false;
        }

#if NET7_0_OR_GREATER
        /// <summary>
        /// Get member cast to type
        /// </summary>
        /// <param name="member">Member name</param>
        /// <typeparam name="T">Target type</typeparam>
        /// <returns>Value</returns>
        /// <exception cref="KeyNotFoundException">If key was not found in members</exception>
        public T GetNumber<T>(string member) where T : INumber<T>
        {
            return LinearCommon.CastNumber<T>(_members[member]) ?? throw new InvalidCastException();
        }

        /// <summary>
        /// Try to get member cast to type
        /// </summary>
        /// <param name="member">Member name</param>
        /// <param name="result">Value</param>
        /// <typeparam name="T">Target type</typeparam>
        /// <returns>True if cast succeeded</returns>
        /// <exception cref="KeyNotFoundException">If key was not found in members</exception>
        public bool TryGetNumber<T>(string member, out T result) where T : INumber<T>
        {
            result = default!;
            try
            {
                if (_members.TryGetValue(member, out object? v) && LinearCommon.CastNumber<T>(v) is { } v2)
                {
                    result = v2;
                    return true;
                }
            }
            catch (OverflowException)
            {
                return false;
            }
            return false;
        }
#endif
    }
}
