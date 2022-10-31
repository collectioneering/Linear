﻿namespace Linear.Runtime;

/// <summary>
/// Context for deserializer.
/// </summary>
/// <param name="Structure">Active structure.</param>
/// <param name="ArrayLength">Array length.</param>
/// <param name="PointerArrayLength">Pointer array length.</param>
/// <param name="PointerOffset">Pointer offset.</param>
public readonly record struct DeserializerContext(StructureInstance Structure, long? ArrayLength = null, long? PointerArrayLength = null, long? PointerOffset = null);
