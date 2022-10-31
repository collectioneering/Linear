using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linear.Utility;

namespace Linear.Runtime;

/// <summary>
/// Standard properties for deserializer.
/// </summary>
/// <param name="ArrayLength">Array length.</param>
/// <param name="PointerArrayLength">Pointer array length.</param>
/// <param name="PointerOffset">Pointer offset.</param>
/// <param name="LittleEndian">Little-endian.</param>
public readonly record struct DeserializerStandardProperties(ExpressionDefinition? ArrayLength = null, ExpressionDefinition? PointerArrayLength = null, ExpressionDefinition? PointerOffset = null, ExpressionDefinition? LittleEndian = null)
{
    /// <summary>
    /// Creates <see cref="DeserializerStandardPropertiesInstance"/>.
    /// </summary>
    /// <returns>Instance of <see cref="DeserializerStandardPropertiesInstance"/>.</returns>
    public DeserializerStandardPropertiesInstance ToInstance()
    {
        return new DeserializerStandardPropertiesInstance(ArrayLength?.GetInstance(), PointerArrayLength?.GetInstance(), PointerOffset?.GetInstance(), LittleEndian?.GetInstance());
    }

    /// <summary>
    /// Gets dependencies.
    /// </summary>
    /// <param name="definition">Definition.</param>
    /// <returns>Dependencies.</returns>
    public IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        var res = Enumerable.Empty<Element>();
        if (ArrayLength is { } arrayLength) res = res.Union(arrayLength.GetDependencies(definition));
        if (PointerArrayLength is { } pointerArrayLength) res = res.Union(pointerArrayLength.GetDependencies(definition));
        if (PointerOffset is { } pointerOffset) res = res.Union(pointerOffset.GetDependencies(definition));
        if (LittleEndian is { } littleEndian) res = res.Union(littleEndian.GetDependencies(definition));
        return res;
    }
}

/// <summary>
/// Standard properties for deserializer.
/// </summary>
/// <param name="ArrayLength">Array length.</param>
/// <param name="PointerArrayLength">Pointer array length.</param>
/// <param name="PointerOffset">Pointer offset.</param>
/// <param name="LittleEndian">Little-endian.</param>
public record DeserializerStandardPropertiesInstance(ExpressionInstance? ArrayLength = null, ExpressionInstance? PointerArrayLength = null, ExpressionInstance? PointerOffset = null, ExpressionInstance? LittleEndian = null)
{
    /// <summary>
    /// Augments existing context.
    /// </summary>
    /// <param name="context">Context to augment.</param>
    /// <param name="structureEvaluationContext">Structure evaluation context.</param>
    /// <param name="stream">Stream.</param>
    /// <returns>Augmented context.</returns>
    public DeserializerContext Augment(DeserializerContext context, StructureEvaluationContext structureEvaluationContext, Stream stream)
    {
        object? arrayLength = ArrayLength?.Evaluate(structureEvaluationContext, stream);
        object? pointerArrayLength = PointerArrayLength?.Evaluate(structureEvaluationContext, stream);
        object? pointerOffset = PointerOffset?.Evaluate(structureEvaluationContext, stream);
        object? littleEndian = LittleEndian?.Evaluate(structureEvaluationContext, stream);
        return Augment(context, arrayLength, pointerArrayLength, pointerOffset, littleEndian);
    }

    /// <summary>
    /// Augments existing context.
    /// </summary>
    /// <param name="context">Context to augment.</param>
    /// <param name="structureEvaluationContext">Structure evaluation context.</param>
    /// <param name="memory">Memory.</param>
    /// <returns>Augmented context.</returns>
    public DeserializerContext Augment(DeserializerContext context, StructureEvaluationContext structureEvaluationContext, ReadOnlyMemory<byte> memory)
    {
        object? arrayLength = ArrayLength?.Evaluate(structureEvaluationContext, memory);
        object? pointerArrayLength = PointerArrayLength?.Evaluate(structureEvaluationContext, memory);
        object? pointerOffset = PointerOffset?.Evaluate(structureEvaluationContext, memory);
        object? littleEndian = LittleEndian?.Evaluate(structureEvaluationContext, memory);
        return Augment(context, arrayLength, pointerArrayLength, pointerOffset, littleEndian);
    }

    /// <summary>
    /// Augments existing context.
    /// </summary>
    /// <param name="context">Context to augment.</param>
    /// <param name="structureEvaluationContext">Structure evaluation context.</param>
    /// <param name="span">Span.</param>
    /// <returns>Augmented context.</returns>
    public DeserializerContext Augment(DeserializerContext context, StructureEvaluationContext structureEvaluationContext, ReadOnlySpan<byte> span)
    {
        object? arrayLength = ArrayLength?.Evaluate(structureEvaluationContext, span);
        object? pointerArrayLength = PointerArrayLength?.Evaluate(structureEvaluationContext, span);
        object? pointerOffset = PointerOffset?.Evaluate(structureEvaluationContext, span);
        object? littleEndian = LittleEndian?.Evaluate(structureEvaluationContext, span);
        return Augment(context, arrayLength, pointerArrayLength, pointerOffset, littleEndian);
    }

    private static DeserializerContext Augment(DeserializerContext context, object? arrayLength, object? pointerArrayLength, object? pointerOffset, object? littleEndian)
    {
        if (arrayLength != null) context = context with { ArrayLength = CastUtil.CastLong(arrayLength) };
        if (pointerArrayLength != null) context = context with { PointerArrayLength = CastUtil.CastLong(pointerArrayLength) };
        if (pointerOffset != null) context = context with { PointerOffset = CastUtil.CastLong(pointerOffset) };
        if (littleEndian != null) context = context with { LittleEndian = CastUtil.CastBool(littleEndian) };
        return context;
    }
}
