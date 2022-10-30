using System;

namespace Linear.Utility;

internal static class CastUtil
{
    internal static byte CastByte(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => (byte)b,
            ushort b => (byte)b,
            short b => (byte)b,
            uint b => (byte)b,
            int b => (byte)b,
            ulong b => (byte)b,
            long b => (byte)b,
            float b => (byte)b,
            double b => (byte)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static sbyte CastSByte(object? number)
    {
        return number switch
        {
            byte b => (sbyte)b,
            sbyte b => b,
            ushort b => (sbyte)b,
            short b => (sbyte)b,
            uint b => (sbyte)b,
            int b => (sbyte)b,
            ulong b => (sbyte)b,
            long b => (sbyte)b,
            float b => (sbyte)b,
            double b => (sbyte)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static ushort CastUShort(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => (ushort)b,
            ushort b => b,
            short b => (ushort)b,
            uint b => (ushort)b,
            int b => (ushort)b,
            ulong b => (ushort)b,
            long b => (ushort)b,
            float b => (ushort)b,
            double b => (ushort)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static short CastShort(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => b,
            ushort b => (short)b,
            short b => b,
            uint b => (short)b,
            int b => (short)b,
            ulong b => (short)b,
            long b => (short)b,
            float b => (short)b,
            double b => (short)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static uint CastUInt(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => (uint)b,
            ushort b => b,
            short b => (uint)b,
            uint b => b,
            int b => (uint)b,
            ulong b => (uint)b,
            long b => (uint)b,
            float b => (uint)b,
            double b => (uint)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static int CastInt(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => b,
            ushort b => b,
            short b => b,
            uint b => (int)b,
            int b => b,
            ulong b => (int)b,
            long b => (int)b,
            float b => (int)b,
            double b => (int)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static ulong CastULong(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => (ulong)b,
            ushort b => b,
            short b => (ulong)b,
            uint b => b,
            int b => (ulong)b,
            ulong b => b,
            long b => (ulong)b,
            float b => (ulong)b,
            double b => (ulong)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static long CastLong(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => b,
            ushort b => b,
            short b => b,
            uint b => b,
            int b => b,
            ulong b => (long)b,
            long b => b,
            float b => (long)b,
            double b => (long)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static float CastFloat(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => b,
            ushort b => b,
            short b => b,
            uint b => b,
            int b => b,
            ulong b => b,
            long b => b,
            float b => b,
            double b => (float)b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static double CastDouble(object? number)
    {
        return number switch
        {
            byte b => b,
            sbyte b => b,
            ushort b => b,
            short b => b,
            uint b => b,
            int b => b,
            ulong b => b,
            long b => b,
            float b => b,
            double b => b,
            _ => throw new InvalidCastException(
                $"Could not cast from type {number?.GetType().FullName} to {typeof(long)}")
        };
    }

    internal static bool TryCastLong(object? number, out long value)
    {
        (long item1, bool item2) = number switch
        {
            byte b => (b, true),
            sbyte b => (b, true),
            ushort b => (b, true),
            short b => (b, true),
            uint b => (b, true),
            int b => (b, true),
            ulong b => ((long)b, true),
            long b => (b, true),
            float b => ((long)b, true),
            double b => ((long)b, true),
            _ => (0, false)
        };
        value = item1;
        return item2;
    }
}
