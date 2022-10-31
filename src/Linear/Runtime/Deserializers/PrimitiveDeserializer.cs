using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fp;
using Linear.Utility;

namespace Linear.Runtime.Deserializers;

/// <summary>
/// Deserializes primitives.
/// </summary>
public class PrimitiveDeserializerDefinition : DeserializerDefinition
{
    private readonly Type _type;

    /// <summary>
    /// Initializes an instance of <see cref="PrimitiveDeserializerDefinition"/>.
    /// </summary>
    /// <param name="type">Target type.</param>
    public PrimitiveDeserializerDefinition(Type type)
    {
        _type = type;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return Enumerable.Empty<Element>();
    }

    /// <inheritdoc />
    public override IDeserializer GetInstance()
    {
        return new PrimitiveDeserializer(_type);
    }
}

/// <summary>
/// Deserializes primitives.
/// </summary>
public class PrimitiveDeserializer : IDeserializer
{
    private readonly Type _type;

    /// <summary>
    /// Initializes an instance of <see cref="PrimitiveDeserializer"/>.
    /// </summary>
    /// <param name="type">Target type.</param>
    public PrimitiveDeserializer(Type type)
    {
        _type = type;
    }

    /// <inheritdoc />
    public string? GetTargetTypeName() => null;

    /// <inheritdoc />
    public Type GetTargetType() => _type;

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, Stream stream, long offset, long? length = null, int index = 0)
    {
        // Possible addition: property group support little endian (requires boolean expressions)
        offset += context.Structure.AbsoluteOffset;
        return Type.GetTypeCode(_type) switch
        {
            TypeCode.Boolean => new DeserializeResult(PrimitiveUtil.ReadBool(stream, offset), 1),
            TypeCode.Byte => new DeserializeResult(PrimitiveUtil.ReadU8(stream, offset), 1),
            TypeCode.Char => new DeserializeResult(PrimitiveUtil.ReadU16(stream, offset, ExtractLittleEndianOrThrow(context)), 2),
            TypeCode.DateTime => throw new NotSupportedException(),
            TypeCode.DBNull => throw new NotSupportedException(),
            TypeCode.Decimal => throw new NotSupportedException(),
            TypeCode.Double => new DeserializeResult(PrimitiveUtil.ReadDouble(stream, offset), 8),
            TypeCode.Empty => throw new NullReferenceException(),
            TypeCode.Int16 => new DeserializeResult(PrimitiveUtil.ReadS16(stream, offset, ExtractLittleEndianOrThrow(context)), 2),
            TypeCode.Int32 => new DeserializeResult(PrimitiveUtil.ReadS32(stream, offset, ExtractLittleEndianOrThrow(context)), 4),
            TypeCode.Int64 => new DeserializeResult(PrimitiveUtil.ReadS64(stream, offset, ExtractLittleEndianOrThrow(context)), 8),
            TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
            TypeCode.SByte => new DeserializeResult(PrimitiveUtil.ReadS8(stream, offset), 1),
            TypeCode.Single => new DeserializeResult(PrimitiveUtil.ReadSingle(stream, offset), 4),
            TypeCode.String => throw new NotSupportedException(), // Not supporting direct
            TypeCode.UInt16 => new DeserializeResult(PrimitiveUtil.ReadU16(stream, offset, ExtractLittleEndianOrThrow(context)), 2),
            TypeCode.UInt32 => new DeserializeResult(PrimitiveUtil.ReadU32(stream, offset, ExtractLittleEndianOrThrow(context)), 4),
            TypeCode.UInt64 => new DeserializeResult(PrimitiveUtil.ReadU64(stream, offset, ExtractLittleEndianOrThrow(context)), 8),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory, long offset, long? length = null, int index = 0)
    {
        return Deserialize(context, memory.Span, offset, length, index);
    }

    /// <inheritdoc />
    public DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span, long offset, long? length = null, int index = 0)
    {
        // Possible addition: property group support little endian (requires boolean expressions)
        LinearUtil.TrimStart(ref span, context.Structure, offset);
        return Type.GetTypeCode(_type) switch
        {
            TypeCode.Boolean => new DeserializeResult(span[0] != 0, 1),
            TypeCode.Byte => new DeserializeResult(span[0], 1),
            TypeCode.Char => new DeserializeResult(Processor.GetU16(span, ExtractLittleEndianOrThrow(context)), 2),
            TypeCode.DateTime => throw new NotSupportedException(),
            TypeCode.DBNull => throw new NotSupportedException(),
            TypeCode.Decimal => throw new NotSupportedException(),
            TypeCode.Double => new DeserializeResult(Processor.GetDouble(span), 8),
            TypeCode.Empty => throw new NullReferenceException(),
            TypeCode.Int16 => new DeserializeResult(Processor.GetS16(span, ExtractLittleEndianOrThrow(context)), 2),
            TypeCode.Int32 => new DeserializeResult(Processor.GetS32(span, ExtractLittleEndianOrThrow(context)), 4),
            TypeCode.Int64 => new DeserializeResult(Processor.GetS64(span, ExtractLittleEndianOrThrow(context)), 8),
            TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
            TypeCode.SByte => new DeserializeResult((sbyte)span[0], 1),
            TypeCode.Single => new DeserializeResult(Processor.GetSingle(span), 4),
            TypeCode.String => throw new NotSupportedException(), // Not supporting direct
            TypeCode.UInt16 => new DeserializeResult(Processor.GetU16(span, ExtractLittleEndianOrThrow(context)), 2),
            TypeCode.UInt32 => new DeserializeResult(Processor.GetU32(span, ExtractLittleEndianOrThrow(context)), 4),
            TypeCode.UInt64 => new DeserializeResult(Processor.GetU64(span, ExtractLittleEndianOrThrow(context)), 8),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private bool ExtractLittleEndianOrThrow(in DeserializerContext context)
    {
        return context.LittleEndian ?? throw new InvalidOperationException("Endianness not specified for deserializer");
    }
}
