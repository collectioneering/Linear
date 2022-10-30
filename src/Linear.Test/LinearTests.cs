using System;
using System.IO;
using Linear.Runtime;
using NUnit.Framework;

namespace Linear.Test
{
    public class LinearTests
    {
        private const string _test1 = """
main {
    var a 4*2+5;
    var b 5+8*9;
    var c 4/2*8;
    var d 9*7/3;
    var e 0xff&0x11;
    ushort f `0;
    var f2 ushort`2;
    byte g `5;
}
""";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var res = new StructureRegistry();
            Assert.That(res.TryLoad(_test1, Console.WriteLine), Is.True);
            Assert.That(res.TryGetStructure("main", out Structure structure), Is.True);
            Assert.That(structure, Is.Not.Null);
            MemoryStream ms = new(new byte[] { 0, 1, 2, 3, 4, 5 });
            StructureInstance si = res.Parse("main", ms);
            Assert.That(si["a"], Is.EqualTo(4 * 2 + 5));
            Assert.That(si["b"], Is.EqualTo(5 + 8 * 9));
            Assert.That(si["c"], Is.EqualTo(4 / 2 * 8));
            Assert.That(si["d"], Is.EqualTo(9 * 7 / 3));
            Assert.That(si["e"], Is.EqualTo(0xff & 0x11));
            Assert.That(si["f"], Is.EqualTo(0x100));
            Assert.That(si["f2"], Is.EqualTo(0x0302));
            Assert.That(si["g"], Is.EqualTo(0x5));
        }
    }
}
