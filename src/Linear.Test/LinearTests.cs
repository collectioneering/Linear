using System;
using System.Collections.Generic;
using System.IO;
using Linear.Runtime;
using NUnit.Framework;

namespace Linear.Test
{
    public class LinearTests
    {
        private const string SrcSpec = """
main {
    var a 4*2+5;
    var b 5+8*9;
    var c 4/2*8;
    var d 9*7/3;
    var e 0xff&0x11;
    ushort f `0;
    ushortb f2 `0;
    var f3 ushort`2;
    byte g `5;

    var tmp 10;
    lambda l1 $$i * tmp;
    var l1_result call_lambda_with_i(l1, 20);

    string vvv `get_dummy_buffer()![5,3];
    string vvv2 `get_dummy_buffer()![5..8];

    buf bu `[2,2];
    lambda l2 $$i + 1;
    var bu2 xor(buf`get_dummy_buffer()![8..12], l2);
    $discard true;
    var discarded 101;
}
""";


        [SetUp]
        public void Setup()
        {
        }

        private static readonly byte[] s_Test1_Data = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };
        private static readonly byte[] s_Test1_Data2 = { 0x00, 0x10, 0x20, 0x30, 0, (byte)'l', (byte)'o', (byte)'l', 0, 0, 0, 0 };

        [Test]
        public void Test1()
        {
            var res = new StructureRegistry();
            res.AddMethod("call_lambda_with_i", static (context, args) =>
            {
                if (args.Length != 2)
                {
                    throw new ArgumentException($"expected <lambda> <i> (actual length {args.Length})");
                }
                if (args[0] is not ExpressionInstance expr)
                {
                    throw new ArgumentException("expected <lambda> <i> (did not receive lambda at position 0)");
                }
                var replacements = new Dictionary<string, object>();
                replacements["i"] = args[1];
                context = context with { LambdaReplacements = replacements };
                return expr.Evaluate(context, ReadOnlySpan<byte>.Empty);
            });
            res.AddMethod("get_dummy_buffer", static (_, _) => s_Test1_Data2);
            Assert.That(res.TryLoad(SrcSpec, Console.WriteLine), Is.True);
            Assert.That(res.TryGetStructure("main", out Structure structure), Is.True);
            Assert.That(structure, Is.Not.Null);
            MemoryStream ms = new(s_Test1_Data);
            StructureInstance si = res.Parse("main", ms);
            Assert.That(si["a"], Is.EqualTo(4 * 2 + 5));
            Assert.That(si["b"], Is.EqualTo(5 + 8 * 9));
            Assert.That(si["c"], Is.EqualTo(4 / 2 * 8));
            Assert.That(si["d"], Is.EqualTo(9 * 7 / 3));
            Assert.That(si["e"], Is.EqualTo(0xff & 0x11));
            Assert.That(si["f"], Is.EqualTo(0x100));
            Assert.That(si["f2"], Is.EqualTo(0x1));
            Assert.That(si["f3"], Is.EqualTo(0x0302));
            Assert.That(si["g"], Is.EqualTo(0x5));
            Assert.That(si["l1_result"], Is.EqualTo(200));
            Assert.That(si["vvv"], Is.EqualTo("lol"));
            Assert.That(si["vvv2"], Is.EqualTo("lol"));
            Assert.That(((ReadOnlyMemory<byte>)si["bu"]).ToArray(), Is.EqualTo(new byte[] { 0x02, 0x03 }));
            Assert.That(((ReadOnlyMemory<byte>)si["bu2"]).ToArray(), Is.EqualTo(new byte[] { 0x01, 0x02, 0x03, 0x04 }));
            Assert.That(si.Contains("discarded"), Is.EqualTo(false));
            si = res.Parse("main", s_Test1_Data);
            Assert.That(si["a"], Is.EqualTo(4 * 2 + 5));
            Assert.That(si["b"], Is.EqualTo(5 + 8 * 9));
            Assert.That(si["c"], Is.EqualTo(4 / 2 * 8));
            Assert.That(si["d"], Is.EqualTo(9 * 7 / 3));
            Assert.That(si["e"], Is.EqualTo(0xff & 0x11));
            Assert.That(si["f"], Is.EqualTo(0x100));
            Assert.That(si["f2"], Is.EqualTo(0x1));
            Assert.That(si["f3"], Is.EqualTo(0x0302));
            Assert.That(si["g"], Is.EqualTo(0x5));
            Assert.That(si["l1_result"], Is.EqualTo(200));
            Assert.That(si["vvv"], Is.EqualTo("lol"));
            Assert.That(si["vvv2"], Is.EqualTo("lol"));
            Assert.That(((ReadOnlyMemory<byte>)si["bu"]).ToArray(), Is.EqualTo(new byte[] { 0x02, 0x03 }));
            Assert.That(((ReadOnlyMemory<byte>)si["bu2"]).ToArray(), Is.EqualTo(new byte[] { 0x01, 0x02, 0x03, 0x04 }));
            Assert.That(si.Contains("discarded"), Is.EqualTo(false));
        }
    }
}
