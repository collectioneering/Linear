using System.Collections.Generic;

namespace Linear.Runtime;

/// <summary>
/// Context for deserializer.
/// </summary>
/// <param name="Structure">Active structure.</param>
/// <param name="Parameters">Active parameters.</param>
/// <param name="LittleEndian">Little-endian.</param>
public readonly record struct DeserializerContext(StructureInstance Structure, Dictionary<string, object>? Parameters = null, bool? LittleEndian = null);
