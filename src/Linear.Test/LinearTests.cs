using System;
using System.IO;
using Linear.Runtime;
using NUnit.Framework;

namespace Linear.Test
{
    public class LinearTests
    {
        private const string _test1 = @"
main {
    var a 4*2+5;
    var b 5+8*9;
    var c 4/2*8;
    var d 9*7/3;
    var e 0xff&0x11;
    ushort f `0;
    var f2 ushort`2;
    byte g `5;
}";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var res = new StructureRegistry();
            Assert.IsTrue(res.TryLoad(new StringReader(_test1), Console.WriteLine));
            Assert.IsNotNull(res);
            Assert.IsTrue(res.TryGetValue("main", out Structure structure));
            MemoryStream ms = new(new byte[] { 0, 1, 2, 3, 4, 5 });
            StructureInstance si = structure.Parse(res, ms, new ParseState("main"));
            Assert.AreEqual(4 * 2 + 5, si["a"]);
            Assert.AreEqual(5 + 8 * 9, si["b"]);
            Assert.AreEqual(4 / 2 * 8, si["c"]);
            Assert.AreEqual(9 * 7 / 3, si["d"]);
            Assert.AreEqual(0xff & 0x11, si["e"]);
            Assert.AreEqual(0x100, si["f"]);
            Assert.AreEqual(0x0302, si["f2"]);
            Assert.AreEqual(0x5, si["g"]);
        }
    }
}
