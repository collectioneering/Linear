#if NET7_0_OR_GREATER
using System.Numerics;

namespace Linear;

internal static class NumberUtil
{
    internal static T? CastNumber<T>(object? value) where T : INumber<T>
    {
        return value switch
        {
            byte v => T.CreateChecked(v),
            sbyte v => T.CreateChecked(v),
            ushort v => T.CreateChecked(v),
            short v => T.CreateChecked(v),
            uint v => T.CreateChecked(v),
            int v => T.CreateChecked(v),
            ulong v => T.CreateChecked(v),
            long v => T.CreateChecked(v),
            float v => T.CreateChecked(v),
            double v => T.CreateChecked(v),
            _ => default(T?)
        };
    }

    internal static T? CastNumberSaturating<T>(object? value) where T : INumber<T>
    {
        return value switch
        {
            byte v => T.CreateSaturating(v),
            sbyte v => T.CreateSaturating(v),
            ushort v => T.CreateSaturating(v),
            short v => T.CreateSaturating(v),
            uint v => T.CreateSaturating(v),
            int v => T.CreateSaturating(v),
            ulong v => T.CreateSaturating(v),
            long v => T.CreateSaturating(v),
            float v => T.CreateSaturating(v),
            double v => T.CreateSaturating(v),
            _ => default(T?)
        };
    }

    internal static T? CastNumberTruncating<T>(object? value) where T : INumber<T>
    {
        return value switch
        {
            byte v => T.CreateTruncating(v),
            sbyte v => T.CreateTruncating(v),
            ushort v => T.CreateTruncating(v),
            short v => T.CreateTruncating(v),
            uint v => T.CreateTruncating(v),
            int v => T.CreateTruncating(v),
            ulong v => T.CreateTruncating(v),
            long v => T.CreateTruncating(v),
            float v => T.CreateTruncating(v),
            double v => T.CreateTruncating(v),
            _ => default(T?)
        };
    }
}
#endif
