using System;
using System.Collections.Generic;
using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Definition of custom deserializer
/// </summary>
public interface IDeserializer
{
    /// <summary>
    /// Get target type name of deserializer
    /// </summary>
    /// <returns>Target type name</returns>
    /// <remarks>
    /// Mandatory for user-defined deserializers
    /// </remarks>
    string? GetTargetTypeName();

    /// <summary>
    /// Get target type of deserializer
    /// </summary>
    /// <returns>Target type</returns>
    Type GetTargetType();

    /// <summary>
    /// Deserialize object
    /// </summary>
    /// <param name="instance">Structure instance</param>
    /// <param name="stream">Stream to read from</param>
    /// <param name="offset">Offset in stream</param>
    /// <param name="littleEndian">Endianness</param>
    /// <param name="standardProperties">Standard properties</param>
    /// <param name="parameters">Deserializer parameters</param>
    /// <param name="length">Length of structure</param>
    /// <param name="index">Array index</param>
    /// <returns>Deserialized object</returns>
    DeserializeResult Deserialize(StructureInstance instance, Stream stream, long offset, bool littleEndian,
        Dictionary<StandardProperty, object>? standardProperties, Dictionary<string, object>? parameters,
        long length = 0, int index = 0);
}