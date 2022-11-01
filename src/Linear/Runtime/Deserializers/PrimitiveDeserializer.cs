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
    private readonly bool _littleEndian;
    private readonly Type _type;

    /// <summary>
    /// Initializes an instance of <see cref="PrimitiveDeserializerDefinition"/>.
    /// </summary>
    /// <param name="type">Target type.</param>
    /// <param name="littleEndian">Little-endian.</param>
    public PrimitiveDeserializerDefinition(Type type, bool littleEndian)
    {
        _type = type;
        _littleEndian = littleEndian;
    }

    /// <inheritdoc />
    public override IEnumerable<Element> GetDependencies(StructureDefinition definition)
    {
        return Enumerable.Empty<Element>();
    }

    /// <inheritdoc />
    public override DeserializerInstance GetInstance()
    {
        return new PrimitiveDeserializer(_type, _littleEndian);
    }
}

/// <summary>
/// Deserializes primitives.
/// </summary>
public class PrimitiveDeserializer : DeserializerInstance
{
    private readonly bool _littleEndian;
    private readonly Type _type;

    /// <summary>
    /// Initializes an instance of <see cref="PrimitiveDeserializer"/>.
    /// </summary>
    /// <param name="type">Target type.</param>
    /// <param name="littleEndian">Little-endian.</param>
    public PrimitiveDeserializer(Type type, bool littleEndian)
    {
        _type = type;
        _littleEndian = littleEndian;
    }

    /// <inheritdoc />
    public override Type GetTargetType() => _type;

    /// <inheritdoc />
    public override DeserializeResult Deserialize(DeserializerContext context, Stream stream, long offset, long? length = null, int? index = null)
    {
        ValidateLength(length, _type);
        // Possible addition: property group support little endian (requires boolean expressions)
        offset += context.Structure.AbsoluteOffset;
        return Type.GetTypeCode(_type) switch
        {
            TypeCode.Boolean => new DeserializeResult(PrimitiveUtil.ReadBool(stream, offset), 1),
            TypeCode.Byte => new DeserializeResult(PrimitiveUtil.ReadU8(stream, offset), 1),
            TypeCode.Char => new DeserializeResult(PrimitiveUtil.ReadU16(stream, offset, _littleEndian), 2),
            TypeCode.DateTime => throw new NotSupportedException(),
            TypeCode.DBNull => throw new NotSupportedException(),
            TypeCode.Decimal => throw new NotSupportedException(),
            TypeCode.Double => new DeserializeResult(PrimitiveUtil.ReadDouble(stream, offset), 8),
            TypeCode.Empty => throw new NullReferenceException(),
            TypeCode.Int16 => new DeserializeResult(PrimitiveUtil.ReadS16(stream, offset, _littleEndian), 2),
            TypeCode.Int32 => new DeserializeResult(PrimitiveUtil.ReadS32(stream, offset, _littleEndian), 4),
            TypeCode.Int64 => new DeserializeResult(PrimitiveUtil.ReadS64(stream, offset, _littleEndian), 8),
            TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
            TypeCode.SByte => new DeserializeResult(PrimitiveUtil.ReadS8(stream, offset), 1),
            TypeCode.Single => new DeserializeResult(PrimitiveUtil.ReadSingle(stream, offset), 4),
            TypeCode.String => throw new NotSupportedException(), // Not supporting direct
            TypeCode.UInt16 => new DeserializeResult(PrimitiveUtil.ReadU16(stream, offset, _littleEndian), 2),
            TypeCode.UInt32 => new DeserializeResult(PrimitiveUtil.ReadU32(stream, offset, _littleEndian), 4),
            TypeCode.UInt64 => new DeserializeResult(PrimitiveUtil.ReadU64(stream, offset, _littleEndian), 8),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <inheritdoc />
    public override DeserializeResult Deserialize(DeserializerContext context, ReadOnlyMemory<byte> memory, long offset, long? length = null, int? index = null)
    {
        return Deserialize(context, memory.Span, offset, length, index);
    }

    /// <inheritdoc />
    public override DeserializeResult Deserialize(DeserializerContext context, ReadOnlySpan<byte> span, long offset, long? length = null, int? index = null)
    {
        ValidateLength(length, _type);
        // Possible addition: property group support little endian (requires boolean expressions)
        LinearUtil.TrimStart(ref span, context.Structure, offset);
        return Type.GetTypeCode(_type) switch
        {
            TypeCode.Boolean => new DeserializeResult(span[0] != 0, 1),
            TypeCode.Byte => new DeserializeResult(span[0], 1),
            TypeCode.Char => new DeserializeResult(Processor.GetU16(span, _littleEndian), 2),
            TypeCode.DateTime => throw new NotSupportedException(),
            TypeCode.DBNull => throw new NotSupportedException(),
            TypeCode.Decimal => throw new NotSupportedException(),
            TypeCode.Double => new DeserializeResult(Processor.GetDouble(span), 8),
            TypeCode.Empty => throw new NullReferenceException(),
            TypeCode.Int16 => new DeserializeResult(Processor.GetS16(span, _littleEndian), 2),
            TypeCode.Int32 => new DeserializeResult(Processor.GetS32(span, _littleEndian), 4),
            TypeCode.Int64 => new DeserializeResult(Processor.GetS64(span, _littleEndian), 8),
            TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
            TypeCode.SByte => new DeserializeResult((sbyte)span[0], 1),
            TypeCode.Single => new DeserializeResult(Processor.GetSingle(span), 4),
            TypeCode.String => throw new NotSupportedException(), // Not supporting direct
            TypeCode.UInt16 => new DeserializeResult(Processor.GetU16(span, _littleEndian), 2),
            TypeCode.UInt32 => new DeserializeResult(Processor.GetU32(span, _littleEndian), 4),
            TypeCode.UInt64 => new DeserializeResult(Processor.GetU64(span, _littleEndian), 8),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static void ValidateLength(long? length, Type type)
    {
        int minimum = Type.GetTypeCode(type) switch
        {
            TypeCode.Boolean => 1,
            TypeCode.Byte => 1,
            TypeCode.Char => 2,
            TypeCode.DateTime => throw new NotSupportedException(),
            TypeCode.DBNull => throw new NotSupportedException(),
            TypeCode.Decimal => throw new NotSupportedException(),
            TypeCode.Double => 8,
            TypeCode.Empty => throw new NullReferenceException(),
            TypeCode.Int16 => 2,
            TypeCode.Int32 => 4,
            TypeCode.Int64 => 8,
            TypeCode.Object => throw new NotSupportedException(), // Not supporting direct
            TypeCode.SByte => 1,
            TypeCode.Single => 4,
            TypeCode.String => throw new NotSupportedException(), // Not supporting direct
            TypeCode.UInt16 => 2,
            TypeCode.UInt32 => 4,
            TypeCode.UInt64 => 8,
            _ => throw new ArgumentOutOfRangeException()
        };
        ValidateLength(length, minimum);
    }

    private static void ValidateLength(long? length, long minimum)
    {
        if (length is { } l && l < minimum)
        {
            throw new ArgumentException($"Cannot extract value of length {minimum} from available size {l}");
        }
    }
}
