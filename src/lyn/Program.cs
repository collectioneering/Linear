using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Fp;
using Linear;
using Linear.Runtime;
using Linear.Runtime.Expressions;

namespace lyn
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    internal static class Program
    {
        private const string MainLayout = "main";

        private static int Main(string[] args)
        {
            //return Parser.Default.ParseArguments<Configuration>(args).MapResult(Run, errors => 1);
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: <formatFile> <input> <outputDir>");
                return 1;
            }

            return Run(new Configuration(args[0], args[1], args[2]));
        }

        private static int Run(Configuration conf)
        {
            List<MethodCallExpression.NamedDelegate> methods;
#if EnableLinearLambda
            methods = new List<MethodCallExpression.NamedDelegate>();
#else
            methods = null;
#endif
            var registry = new StructureRegistry(null, methods);
            using (StreamReader sr = File.OpenText(conf.LayoutFile!))
                if (!registry.TryLoad(sr, Console.WriteLine))
                {
                    Console.WriteLine("Errors occurred while parsing structure file, aborting.");
                    return 5;
                }

            if (!registry.TryGetValue(MainLayout, out Structure? structure))
            {
                Console.WriteLine($"Failed to find structure named {MainLayout}");
                return 2;
            }

            if (File.Exists(conf.Input))
            {
                return OperateFile(registry, structure, conf.Input!, conf.OutputDir!);
            }
            if (Directory.Exists(conf.Input))
            {
                foreach (string file in Directory.GetFiles(conf.Input!))
                {
                    int resCode = OperateFile(registry, structure, file, Path.Combine(conf.OutputDir!, Path.GetFileName(file)));
                    if (resCode != 0) return resCode;
                }
            }

            Console.WriteLine($"Input {conf.Input} not found");
            return 4;
        }

        private static int OperateFile(StructureRegistry registry, Structure structure, string inputFile, string outputDir)
        {
            using Stream baseStream = File.OpenRead(inputFile);
            using MultiBufferStream mbs = new MultiBufferStream(baseStream);
            StructureInstance si = structure.Parse(registry, mbs, 0, null, baseStream.Length);
            Dictionary<string, IExporter> exporterDictionary = LinearUtil.CreateDefaultExporterDictionary();
            foreach (var output in si.GetOutputs())
            {
                if (!exporterDictionary.TryGetValue(output.Format, out IExporter? exporter))
                {
                    Console.WriteLine($"Failed to find exporter named {output.Format}");
                    return 3;
                }

                string file = Path.Combine(outputDir, output.Name);
                string dir = Path.GetDirectoryName(file) ??
                             throw new ApplicationException("Invalid output file, cannot be root");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                Console.WriteLine(file);
                using FileStream ofs = File.Create(file);
                exporter.Export(baseStream, output.Structure, output.Range, output.Parameters, ofs);
            }

            return 0;
        }

        private class Configuration
        {
            //[Value(0, Required = true, MetaName = nameof(LayoutFile))]
            public string? LayoutFile { get; set; }

            //[Value(1, Required = true, MetaName = nameof(Input))]
            public string? Input { get; set; }

            //[Value(2, Required = true, MetaName = nameof(OutputDir))]
            public string? OutputDir { get; set; }

            internal Configuration(string layoutFile, string input, string outputDir)
            {
                LayoutFile = layoutFile;
                Input = input;
                OutputDir = outputDir;
            }
        }
    }
}
