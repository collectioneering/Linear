using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using CommandLine;
using Esper;
using Linear;
using Linear.Runtime;

namespace lyn
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    internal static class Program
    {

        private static int Main(string[] args) =>
            Parser.Default.ParseArguments<Configuration>(args).MapResult(Run, errors => 1);

        private static int Run(Configuration conf)
        {
            StructureRegistry registry;
            using (FileStream fs = File.OpenRead(conf.LayoutFile))
                registry = LinearUtil.GenerateRegistry(fs);
            if (!registry.TryGetValue(LinearUtil.MainLayout, out Structure structure))
            {
                Console.WriteLine($"Failed to find structure named {LinearUtil.MainLayout}");
                return 2;
            }

            using Stream baseStream = File.OpenRead(conf.InputFile);
            using MultiBufferStream mbs = new MultiBufferStream(baseStream);
            StructureInstance si = structure.Parse(registry, mbs, 0, baseStream.Length, null);
            return 0;
        }

        private class Configuration
        {
            [Value(0, Required = true, MetaName = nameof(LayoutFile))] public string LayoutFile { get; set; }
            [Value(1, Required = true, MetaName = nameof(InputFile))] public string InputFile { get; set; }
            [Value(2, Required = true, MetaName = nameof(OutputDir))] public string OutputDir { get; set; }
        }
    }
}
