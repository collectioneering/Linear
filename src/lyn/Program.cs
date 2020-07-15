﻿using System;
using System.Collections.Generic;
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
            Dictionary<string, IExporter> exporterRegistry = LinearUtil.CreateDefaultExporterRegistry();
            foreach (var res in si.GetOutputs())
            {
                if (!exporterRegistry.TryGetValue(res.format, out IExporter exporter))
                {
                    Console.WriteLine($"Failed to find exporter named {res.format}");
                    return 3;
                }

                string file = Path.Combine(conf.OutputDir, res.name);
                string dir = Path.GetDirectoryName(file);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                using FileStream ofs = File.Create(file);
                exporter.Export(baseStream, res.instance, res.range, res.parameters, ofs);
            }

            return 0;
        }

        private class Configuration
        {
            [Value(0, Required = true, MetaName = nameof(LayoutFile))]
            public string LayoutFile { get; set; }

            [Value(1, Required = true, MetaName = nameof(InputFile))]
            public string InputFile { get; set; }

            [Value(2, Required = true, MetaName = nameof(OutputDir))]
            public string OutputDir { get; set; }
        }
    }
}
