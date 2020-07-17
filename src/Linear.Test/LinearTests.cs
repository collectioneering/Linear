using System.IO;
using Linear.Runtime;
using NUnit.Framework;

namespace Linear.Test
{
    public class LinearTests
    {
        private const string _test1 = @"
main {
    $value a 4*2+5;
    $value b 5+8*9;
    $value c 4/2*8;
    $value d 9*7/3;
    $value e 0xff&0x11;
}";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            StructureRegistry res = LinearCommon.GenerateRegistry(new StringReader(_test1));
            Assert.IsTrue(res.TryGetValue(LinearCommon.MainLayout, out Structure structure));
            MemoryStream ms = new MemoryStream();
            StructureInstance si = structure.Parse(res, ms);
            Assert.AreEqual(4 * 2 + 5, si["a"]);
            Assert.AreEqual(5 + 8 * 9, si["b"]);
            Assert.AreEqual(4 / 2 * 8, si["c"]);
            Assert.AreEqual(9 * 7 / 3, si["d"]);
            Assert.AreEqual(0xff & 0x11, si["e"]);
        }
    }
}
