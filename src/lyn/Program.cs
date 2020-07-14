using System.Diagnostics.CodeAnalysis;
using System.IO;
using CommandLine;
using Linear;

namespace lyn
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    internal static class Program
    {
        private static int Main(string[] args) =>
            Parser.Default.ParseArguments<Configuration>(args).MapResult(Run, errors => 1);

        private static int Run(Configuration conf)
        {
            using (FileStream fs = File.OpenRead(conf.Input))
                LinearUtil.Generate(fs);
            return 0;
        }

        private class Configuration
        {
            [Value(0, Required = true, MetaName = nameof(Input))] public string Input { get; set; }
        }
    }
}
