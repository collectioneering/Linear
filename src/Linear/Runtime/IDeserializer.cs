using System;
using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Definition of custom deserializer
/// </summary>
public interface IDeserializer
{
    /// <summary>
    /// Gets target type name of deserializer.
    /// </summary>
    /// <returns>Target type name.</returns>
    /// <remarks>
    /// Mandatory for user-defined deserializers.
    /// </remarks>
    string? GetTargetTypeName();

    /// <summary>
    /// Gets target type of deserializer.
    /// </summary>
    /// <returns>Target type.</returns>
    Type GetTargetType();

    /// <summary>
    /// Deserializes object.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="stream">Stream to read from.</param>
    /// <param name="offset">Offset in stream.</param>
    /// <param name="littleEndian">Endianness.</param>
    /// <param name="length">Length of structure.</param>
    /// <param name="index">Array index.</param>
    /// <returns>Deserialized object.</returns>
    DeserializeResult Deserialize(DeserializerContext context, Stream stream, long offset, bool littleEndian, long? length = null, int index = 0);

    /// <summary>
    /// Deserializes object.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="memory">Buffer to read from.</param>
    /// <param name="offset">Offset in stream.</param>
    /// <param name="littleEndian">Endianness.</param>
    /// <param name="length">Length of structure.</param>
    /// <param name="index">Array index.</param>
    /// <returns>Deserialized object.</returns>
    DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory, long offset, bool littleEndian, long? length = null, int index = 0);

    /// <summary>
    /// Deserializes object.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="span">Buffer to read from.</param>
    /// <param name="offset">Offset in stream.</param>
    /// <param name="littleEndian">Endianness.</param>
    /// <param name="length">Length of structure.</param>
    /// <param name="index">Array index.</param>
    /// <returns>Deserialized object.</returns>
    DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span, long offset, bool littleEndian, long? length = null, int index = 0);
}
