# Linear
 Linear file structure ingest library

[![NuGet](https://img.shields.io/nuget/v/Linear.svg)](https://www.nuget.org/packages/Linear/)

This library uses structure specification files to read
structured data from files. It is built for .NET Standard 2.0.

The command-line program [lyn](src/lyn) provides a simpe means for using
a structure file to lay out an input file and dump its outputs.

Usage: `lyn <structureFile> <inputFile> <outputDir>`

For structure file examples, see [the sample folder](sample).
Also check [the grammar file](spec/Linear.g4) which has comments for significant elements.

[ANTLR4](https://github.com/antlr/antlr4) generates the structure file parser.