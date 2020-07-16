using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linear.Runtime.Elements
{
    /// <summary>
    /// Expression from offset
    /// </summary>
    public class DataElement : Element
    {
        private readonly string _name;
        private readonly ExpressionDefinition _offsetDefinition;
        private readonly ExpressionDefinition _littleEndianDefinition;
        private readonly IDeserializer _deserializer;
        private readonly Dictionary<string, ExpressionDefinition> _deserializerParams;

        /// <summary>
        /// Create new instance of <see cref="DataElement"/>
        /// </summary>
        /// <param name="name">Name of element</param>
        /// <param name="offsetDefinition">Offset value definition</param>
        /// <param name="littleEndianDefinition">Endianness value definition</param>
        /// <param name="deserializer">Custom deserializer</param>
        /// <param name="deserializerParams">Deserializer parameters</param>
        public DataElement(string name, ExpressionDefinition offsetDefinition,
            ExpressionDefinition littleEndianDefinition, IDeserializer deserializer,
            Dictionary<string, ExpressionDefinition> deserializerParams)
        {
            _name = name;
            _offsetDefinition = offsetDefinition;
            _littleEndianDefinition = littleEndianDefinition;
            _deserializer = deserializer;
            _deserializerParams = deserializerParams;
        }


        /// <inheritdoc />
        public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
        {
            IEnumerable<Element> deps = _offsetDefinition.GetDependencies(definition)
                .Union(_littleEndianDefinition.GetDependencies(definition));
            if (_deserializerParams != null)
                deps = deps.Union(_deserializerParams.SelectMany(kvp => kvp.Value.GetDependencies(definition)));
            return deps;
        }

        /// <inheritdoc />
        public override Action<StructureInstance, Stream, byte[]> GetDelegate()
        {
            Func<StructureInstance, Stream, byte[], object?> srcDelegate = _offsetDefinition.GetDelegate();
            Func<StructureInstance, Stream, byte[], object?>
                littleEndianDelegate = _littleEndianDefinition.GetDelegate();
            Dictionary<string, Func<StructureInstance, Stream, byte[], object?>> deserializerParamsCompact =
                new Dictionary<string, Func<StructureInstance, Stream, byte[], object?>>();
            foreach (var kvp in _deserializerParams)
                deserializerParamsCompact[kvp.Key] = kvp.Value.GetDelegate();
            return (instance, stream, tempBuffer) =>
            {
                Dictionary<string, object> deserializerParams = new Dictionary<string, object>();
                foreach (var kvp in deserializerParamsCompact)
                    deserializerParams[kvp.Key] =
                        kvp.Value(instance, stream, tempBuffer) ?? throw new NullReferenceException();
                object? offset = srcDelegate(instance, stream, tempBuffer);
                object? littleEndian = littleEndianDelegate(instance, stream, tempBuffer);
                if (!LinearUtil.TryCast(offset, out long offsetValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {offset?.GetType().FullName} to type {nameof(Int64)}");
                if (!LinearUtil.TryCast(littleEndian, out bool littleEndianValue))
                    throw new InvalidCastException(
                        $"Could not cast expression of type {littleEndian?.GetType().FullName} to type {nameof(Boolean)}");
                offsetValue += instance.AbsoluteOffset;
                instance.SetMember(_name, _deserializer.Deserialize(instance, stream, tempBuffer, offsetValue,
                    littleEndianValue, deserializerParams));
            };
        }
    }
}
