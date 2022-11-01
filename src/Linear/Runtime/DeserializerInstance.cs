using System;
using System.IO;

namespace Linear.Runtime;

/// <summary>
/// Definition of custom deserializer.
/// </summary>
public abstract class DeserializerInstance
{
    /// <summary>
    /// Gets target type of deserializer.
    /// </summary>
    /// <returns>Target type.</returns>
    public abstract Type GetTargetType();

    /// <summary>
    /// Deserializes object.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="stream">Stream to read from.</param>
    /// <param name="offset">Offset in stream.</param>
    /// <param name="length">Length of structure.</param>
    /// <param name="index">Array index.</param>
    /// <returns>Deserialized object.</returns>
    public abstract DeserializeResult Deserialize(DeserializerContext context, Stream stream, long offset, long? length = null, int? index = null);

    /// <summary>
    /// Deserializes object.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="memory">Buffer to read from.</param>
    /// <param name="offset">Offset in stream.</param>
    /// <param name="length">Length of structure.</param>
    /// <param name="index">Array index.</param>
    /// <returns>Deserialized object.</returns>
    public abstract DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory, long offset, long? length = null, int? index = null);

    /// <summary>
    /// Deserializes object.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="span">Buffer to read from.</param>
    /// <param name="offset">Offset in stream.</param>
    /// <param name="length">Length of structure.</param>
    /// <param name="index">Array index.</param>
    /// <returns>Deserialized object.</returns>
    public abstract DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span, long offset, long? length = null, int? index = null);
}
