using System.Collections.Generic;

namespace Linear.Runtime;

/// <summary>
/// Context for deserializer.
/// </summary>
/// <param name="Structure">Active structure.</param>
/// <param name="Parameters">Active parameters..</param>
/// <param name="ArrayLength">Array length.</param>
/// <param name="PointerArrayLength">Pointer array length.</param>
/// <param name="PointerOffset">Pointer offset.</param>
/// <param name="LittleEndian">Little-endian.</param>
public readonly record struct DeserializerContext(StructureInstance Structure, Dictionary<string, object>? Parameters = null, long? ArrayLength = null, long? PointerArrayLength = null, long? PointerOffset = null, bool? LittleEndian = null);
