using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Linear.Utility;

internal static class PrimitiveUtil
{
    internal static unsafe bool ReadBool(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        stream.ReadExactly(temp);
        return temp[0] != 0;
    }

    internal static async ValueTask<bool> ReadBoolAsync(Stream stream, long offset, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(1);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 1), cancellationToken);
            return array[0] != 0;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe byte ReadU8(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        stream.ReadExactly(temp);
        return temp[0];
    }

    internal static async ValueTask<byte> ReadU8Async(Stream stream, long offset, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(1);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 1), cancellationToken);
            return array[0];
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe sbyte ReadS8(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[1];
        stream.ReadExactly(temp);
        return (sbyte)temp[0];
    }

    internal static async ValueTask<sbyte> ReadS8Async(Stream stream, long offset, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(1);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 1), cancellationToken);
            return (sbyte)array[0];
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe ushort ReadU16(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[2];
        stream.ReadExactly(temp);
        return littleEndian ? BinaryPrimitives.ReadUInt16LittleEndian(temp) : BinaryPrimitives.ReadUInt16BigEndian(temp);
    }

    internal static async ValueTask<ushort> ReadU16Async(Stream stream, long offset, bool littleEndian, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(2);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 2), cancellationToken);
            return littleEndian ? BinaryPrimitives.ReadUInt16LittleEndian(array) : BinaryPrimitives.ReadUInt16BigEndian(array);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe short ReadS16(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[2];
        stream.ReadExactly(temp);
        return littleEndian ? BinaryPrimitives.ReadInt16LittleEndian(temp) : BinaryPrimitives.ReadInt16BigEndian(temp);
    }

    internal static async ValueTask<short> ReadS16Async(Stream stream, long offset, bool littleEndian, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(2);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 2), cancellationToken);
            return littleEndian ? BinaryPrimitives.ReadInt16LittleEndian(array) : BinaryPrimitives.ReadInt16BigEndian(array);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe uint ReadU32(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        stream.ReadExactly(temp);
        return littleEndian ? BinaryPrimitives.ReadUInt32LittleEndian(temp) : BinaryPrimitives.ReadUInt32BigEndian(temp);
    }

    internal static async ValueTask<uint> ReadU32Async(Stream stream, long offset, bool littleEndian, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(4);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 4), cancellationToken);
            return littleEndian ? BinaryPrimitives.ReadUInt32LittleEndian(array) : BinaryPrimitives.ReadUInt32BigEndian(array);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe int ReadS32(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        stream.ReadExactly(temp);
        return littleEndian ? BinaryPrimitives.ReadInt32LittleEndian(temp) : BinaryPrimitives.ReadInt32BigEndian(temp);
    }

    internal static async ValueTask<int> ReadS32Async(Stream stream, long offset, bool littleEndian, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(4);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 4), cancellationToken);
            return littleEndian ? BinaryPrimitives.ReadInt32LittleEndian(array) : BinaryPrimitives.ReadInt32BigEndian(array);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe ulong ReadU64(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        stream.ReadExactly(temp);
        return littleEndian ? BinaryPrimitives.ReadUInt64LittleEndian(temp) : BinaryPrimitives.ReadUInt64BigEndian(temp);
    }

    internal static async ValueTask<ulong> ReadU64Async(Stream stream, long offset, bool littleEndian, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(8);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 8), cancellationToken);
            return littleEndian ? BinaryPrimitives.ReadUInt64LittleEndian(array) : BinaryPrimitives.ReadUInt64BigEndian(array);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe long ReadS64(Stream stream, long offset, bool littleEndian)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        stream.ReadExactly(temp);
        return littleEndian ? BinaryPrimitives.ReadInt64LittleEndian(temp) : BinaryPrimitives.ReadInt64BigEndian(temp);
    }

    internal static async ValueTask<long> ReadS64Async(Stream stream, long offset, bool littleEndian, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(8);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 8), cancellationToken);
            return littleEndian ? BinaryPrimitives.ReadInt64LittleEndian(array) : BinaryPrimitives.ReadInt64BigEndian(array);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe float ReadSingle(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[4];
        stream.ReadExactly(temp);
        return MemoryMarshal.Read<float>(temp);
    }

    internal static async ValueTask<float> ReadSingleAsync(Stream stream, long offset, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(4);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 4), cancellationToken);
            return MemoryMarshal.Read<float>(array);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    internal static unsafe double ReadDouble(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        Span<byte> temp = stackalloc byte[8];
        stream.ReadExactly(temp);
        return MemoryMarshal.Read<double>(temp);
    }

    internal static async ValueTask<double> ReadDoubleAsync(Stream stream, long offset, CancellationToken cancellationToken = default)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        byte[] array = ArrayPool<byte>.Shared.Rent(8);
        try
        {
            await stream.ReadExactlyAsync(array.AsMemory(0, 8), cancellationToken);
            return MemoryMarshal.Read<double>(array);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }
}
