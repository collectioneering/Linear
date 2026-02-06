using System.CommandLine;
using lyn;

var rootCommand = new RootCommand("Use linear format specification files to process files");
rootCommand.Add(new ExportCommand());
var parseResult = rootCommand.Parse(args);
parseResult.InvocationConfiguration.Output = Console.Error;
parseResult.InvocationConfiguration.Error = Console.Error;
return await parseResult.InvokeAsync();
