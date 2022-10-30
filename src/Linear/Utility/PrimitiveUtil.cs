using System;
using System.IO;
using Fp;

namespace Linear.Utility;

internal static class PrimitiveUtil
{
    internal static unsafe bool ReadBool(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        Processor.Read(stream, temp, false);
        return temp[0] != 0;
    }

    internal static unsafe byte ReadU8(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        Processor.Read(stream, temp, false);
        return temp[0];
    }

    internal static unsafe sbyte ReadS8(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        Processor.Read(stream, temp, false);
        return (sbyte)temp[0];
    }

    internal static unsafe ushort ReadU16(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[2];
        Processor.Read(stream, temp, false);
        return Processor.GetU16(temp, littleEndian);
    }

    internal static unsafe short ReadS16(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[2];
        Processor.Read(stream, temp, false);
        return Processor.GetS16(temp, littleEndian);
    }

    internal static unsafe uint ReadU32(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        Processor.Read(stream, temp, false);
        return Processor.GetU32(temp, littleEndian);
    }

    internal static unsafe int ReadS32(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        Processor.Read(stream, temp, false);
        return Processor.GetS32(temp, littleEndian);
    }

    internal static unsafe ulong ReadU64(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        Processor.Read(stream, temp, false);
        return Processor.GetU64(temp, littleEndian);
    }

    internal static unsafe long ReadS64(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        Processor.Read(stream, temp, false);
        return Processor.GetS64(temp, littleEndian);
    }

    internal static unsafe float ReadSingle(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        Processor.Read(stream, temp, false);
        return Processor.GetSingle(temp);
    }

    internal static unsafe double ReadDouble(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        Processor.Read(stream, temp, false);
        return Processor.GetDouble(temp);
    }
}
