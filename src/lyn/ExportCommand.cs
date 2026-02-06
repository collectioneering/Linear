using System.CommandLine;
using Fp;
using Linear.Runtime;
using Linear.Utility;

namespace lyn
{
    internal sealed class ExportCommand : Command
    {
        private readonly Argument<FileInfo> _layoutFileArgument;
        private readonly Argument<FileSystemInfo> _inputArgument;
        private readonly Argument<DirectoryInfo> _outputDirectoryArgument;

        public ExportCommand() : this("export", "Export files based on input format file")
        {
        }

        public ExportCommand(string name, string? description = null) : base(name, description)
        {
            _layoutFileArgument = new Argument<FileInfo>("layoutFile") { HelpName = "layoutFile", Description = "Layout file describing expected format", Arity = ArgumentArity.ExactlyOne };
            Add(_layoutFileArgument);
            _inputArgument = new Argument<FileSystemInfo>("input") { HelpName = "input", Description = "Input content", Arity = ArgumentArity.ExactlyOne };
            Add(_inputArgument);
            _outputDirectoryArgument = new Argument<DirectoryInfo>("outputDirectory") { HelpName = "outputDirectory", Description = "Output directory", Arity = ArgumentArity.ExactlyOne };
            Add(_outputDirectoryArgument);
            SetAction(RunInternalAsync);
        }

        private async Task<int> RunInternalAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
        {
            FileInfo layoutFile = parseResult.GetRequiredValue(_layoutFileArgument);
            FileSystemInfo input = parseResult.GetRequiredValue(_inputArgument);
            DirectoryInfo outputDirectory = parseResult.GetRequiredValue(_outputDirectoryArgument);
            var registry = new StructureRegistry();
            using (StreamReader sr = layoutFile.OpenText())
            {
                if (!registry.TryLoad(sr, Console.WriteLine, layoutFile.FullName))
                {
                    Console.WriteLine("Errors occurred while parsing structure file, aborting.");
                    return 5;
                }
            }
            if (!registry.TryGetStructure(LynUtility.MainLayout, out Structure? mainStructure))
            {
                Console.WriteLine($"Failed to find structure named {LynUtility.MainLayout}");
                return 2;
            }
            if (input is FileInfo { Exists: true })
            {
                return await OperateFileAsync(registry, mainStructure, input.FullName, outputDirectory.FullName, cancellationToken);
            }
            if (input is DirectoryInfo { Exists: true } inputDirectory)
            {
                foreach (FileInfo file in inputDirectory.GetFiles())
                {
                    int resCode = await OperateFileAsync(registry, mainStructure, file.FullName, Path.Combine(outputDirectory.FullName, file.Name), cancellationToken);
                    if (resCode != 0) return resCode;
                }
            }
            Console.WriteLine($"Input {input.FullName} not found");
            return 4;
        }

        private static async Task<int> OperateFileAsync(StructureRegistry registry, Structure mainStructure, string inputFile, string outputDir, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($">>{inputFile}");
            await using Stream baseStream = File.OpenRead(inputFile);
            await using MultiBufferStream mbs = new(baseStream);
            StructureInstance si = await registry.ParseAsync(mainStructure, mbs, cancellationToken);
            Dictionary<string, IExporter> exporterDictionary = LinearUtil.CreateDefaultExporterDictionary();
            foreach (var output in si.GetOutputs())
            {
                if (!exporterDictionary.TryGetValue(output.Format, out IExporter? exporter))
                {
                    Console.WriteLine($"Failed to find exporter named {output.Format}");
                    return 3;
                }

                string file = Path.Combine(outputDir, output.Name);
                string dir = Path.GetDirectoryName(file) ?? throw new ApplicationException("Invalid output file, cannot be root");
                Directory.CreateDirectory(dir);
                Console.WriteLine(file);
                await using FileStream ofs = File.Create(file);
                await exporter.ExportAsync(baseStream, output.Structure, output.Range, output.Parameters, ofs, cancellationToken);
            }

            return 0;
        }
    }
}
