using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linear.Utility;

namespace Linear.Runtime;

/// <summary>
/// Standard properties for deserializer.
/// </summary>
/// <param name="LittleEndian">Little-endian.</param>
public readonly record struct DeserializerStandardProperties(ExpressionDefinition? LittleEndian = null)
{
    /// <summary>
    /// Creates <see cref="DeserializerStandardPropertiesInstance"/>.
    /// </summary>
    /// <returns>Instance of <see cref="DeserializerStandardPropertiesInstance"/>.</returns>
    public DeserializerStandardPropertiesInstance ToInstance()
    {
        return new DeserializerStandardPropertiesInstance(LittleEndian?.GetInstance());
    }

    /// <summary>
    /// Gets dependencies.
    /// </summary>
    /// <param name="definition">Definition.</param>
    /// <returns>Dependencies.</returns>
    public IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        var res = Enumerable.Empty<Element>();
        if (LittleEndian is { } littleEndian) res = res.Union(littleEndian.GetDependencies(definition));
        return res;
    }
}

/// <summary>
/// Standard properties for deserializer.
/// </summary>
/// <param name="LittleEndian">Little-endian.</param>
public record DeserializerStandardPropertiesInstance(ExpressionInstance? LittleEndian = null)
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
        object? littleEndian = LittleEndian?.Evaluate(structureEvaluationContext, stream);
        return Augment(context, littleEndian);
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
        object? littleEndian = LittleEndian?.Evaluate(structureEvaluationContext, memory);
        return Augment(context, littleEndian);
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
        object? littleEndian = LittleEndian?.Evaluate(structureEvaluationContext, span);
        return Augment(context, littleEndian);
    }

    private static DeserializerContext Augment(DeserializerContext context, object? littleEndian)
    {
        if (littleEndian != null) context = context with { LittleEndian = CastUtil.CastBool(littleEndian) };
        return context;
    }
}
