using System;
using NUnit.Framework;

namespace Linear.Test;

public class ArrayTests : TestsBase
{
    [Test]
    public void Array_Redirected_Correct()
    {
        AddBufferMethod("getbuf", new byte[]
        {
            0x01, 0x00, 0x00, 0x00, //
            0x02, 0x00, 0x00, 0x00, //
            0x03, 0x00, 0x00, 0x00, //
            0x04, 0x00, 0x00, 0x00, //
        });
        var res = Create(
            """
main {
    int[4] arr getbuf()!0;
}
"""
        ).Parse("main", ReadOnlySpan<byte>.Empty);
        Assert.That(res["arr"], Is.EqualTo(new int[] { 0x01, 0x02, 0x03, 0x04 }));
    }

    [Test]
    public void PointerArray_BaseRedirected_Correct()
    {
        AddBufferMethod("getbuf", new byte[]
        {
            0x04, 0x00, 0x00, 0x00, //
            0x08, 0x00, 0x00, 0x00, //
            0x0C, 0x00, 0x00, 0x00, //
            0x10, 0x00, 0x00, 0x00, //
        });
        var res = Create(
            """
main {
    int[4] -> int[] arr getbuf()!0,0;
}
"""
        ).Parse("main", new byte[]
        {
            0x05, 0x00, 0x00, 0x00, //
            0x06, 0x00, 0x00, 0x00, //
            0x07, 0x00, 0x00, 0x00, //
            0x08, 0x00, 0x00, 0x00, //
            0x09, 0x00, 0x00, 0x00, //
        });
        Assert.That(res["arr"], Is.EqualTo(new int[] { 0x06, 0x07, 0x08, 0x09 }));
    }

    [Test]
    public void PointerArray_TargetRedirected_Correct()
    {
        AddBufferMethod("getbuf2", new byte[]
        {
            0x05, 0x00, 0x00, 0x00, //
            0x06, 0x00, 0x00, 0x00, //
            0x07, 0x00, 0x00, 0x00, //
            0x08, 0x00, 0x00, 0x00, //
            0x09, 0x00, 0x00, 0x00, //
        });
        var res = Create(
            """
main {
    int[4] -> int[] arr 0,getbuf2()!0;
}
"""
        ).Parse("main", new byte[]
        {
            0x04, 0x00, 0x00, 0x00, //
            0x08, 0x00, 0x00, 0x00, //
            0x0C, 0x00, 0x00, 0x00, //
            0x10, 0x00, 0x00, 0x00, //
        });
        Assert.That(res["arr"], Is.EqualTo(new int[] { 0x06, 0x07, 0x08, 0x09 }));
    }

    [Test]
    public void PointerArray_BaseAndTargetRedirected_Correct()
    {
        AddBufferMethod("getbuf", new byte[]
        {
            0x04, 0x00, 0x00, 0x00, //
            0x08, 0x00, 0x00, 0x00, //
            0x0C, 0x00, 0x00, 0x00, //
            0x10, 0x00, 0x00, 0x00, //
        });
        AddBufferMethod("getbuf2", new byte[]
        {
            0x05, 0x00, 0x00, 0x00, //
            0x06, 0x00, 0x00, 0x00, //
            0x07, 0x00, 0x00, 0x00, //
            0x08, 0x00, 0x00, 0x00, //
            0x09, 0x00, 0x00, 0x00, //
        });
        var res = Create(
            """
main {
    int[4] -> int[] arr getbuf()!0,getbuf2()!0;
}
"""
        ).Parse("main", ReadOnlySpan<byte>.Empty);
        Assert.That(res["arr"], Is.EqualTo(new int[] { 0x06, 0x07, 0x08, 0x09 }));
    }
}
