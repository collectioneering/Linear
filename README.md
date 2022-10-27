# Linear
[![NuGet](https://badgen.net/nuget/v/Linear)](https://www.nuget.org/packages/Linear/) ![.NET Core](https://github.com/collectioneering/Linear/workflows/.NET/badge.svg)

 Linear file structure ingest library

This library uses structure specification files to read
structured data from files. It is built for .NET Standard 2.0.

The command-line program [lyn](src/lyn) provides a simple means for using
a structure file to lay out an input file and dump its outputs.

Usage: `lyn <structureFile> <inputFile> <outputDir>`

Builds (requires [.NET 6 runtime](https://get.dot.net/)) are generated by [this workflow.](https://github.com/collectioneering/Linear/actions)

For structure file examples, see [the sample folder](sample).
Also check [the grammar file](spec/Linear.g4) which has comments for significant elements.

[ANTLR4](https://github.com/antlr/antlr4) generates the structure file parser.

## Writing structure files

#### Example

```
// Structure files follow a simple format.
// For lyn command-line programs, a 'main' struct is needed.
main {
    // '`' means deserialize. Type is taken from type marker intb.
    intb numEntries `0x4;

    // Plain array of 'file' objects starting at 0x10, numEntries elements.
    file[numEntries] entries 0x10;
}

// Structure named 'file' with raw size 0x8 (used in plain arrays)
file 0x8 {
    // Can use explicit deserializer name to get
    // data with 'intb' deserializer at offset 0.
    var offsetChunk intb`0;

    intb length `0x4;
    
    // Variable depending on offsetChunk.
    int offset (offsetChunk * 0x800);
    
    // Call registered function 'log' with another call to 'format' (uses string.Format).
    // $i is a replacement for the array index.
    $call log(format("Entry {0} offset {1} length {2}", $i, offset, length));

    // Export content using the 'data' (direct) exporter.
    // Format name (again using string.Format).
    // $a is a replacement for the absolute position of the structure in the stream.
    $output data [offset - $a, length: length] format("{0:D6}.dat", $i);
}
```

#### Ranges

Some deserializers like string and string16 need a range instead of an offset.

These ranges are expressed as `[<sourceIdx>, end: <endIdxNonInclusive>]` and `[<sourceIdx>, length: <length>]`

#### Standard deserializers

| Type    | Description                     |
|---------|---------------------------------|
| byte      | 8-bit unsigned integer (LE)   |
| ushort    | 16-bit unsigned integer (LE)  |
| uint      | 32-bit unsigned integer (LE)  |
| ulong     | 64-bit unsigned integer (LE   |
| sbyte     | 8-bit signed integer (LE)     |
| short     | 16-bit signed integer (LE)    |
| int       | 32-bit signed integer (LE)    |
| long      | 64-bit signed integer (LE)    |
| byteb     | 8-bit unsigned integer (BE)   |
| ushortb   | 16-bit unsigned integer (BE)  |
| uintb     | 32-bit unsigned integer (BE)  |
| ulongb    | 64-bit unsigned integer (BE)  |
| sbyteb    | 8-bit signed integer  (BE)    |
| shortb    | 16-bit signed integer (BE)    |
| intb      | 32-bit signed integer (BE)    |
| longb     | 64-bit signed integer (BE)    |
| float     | 32-bit floating point         |
| double    | 64-bit floating point         |
| string    | UTF-8 string                  |
| cstring   | Null-terminated UTF-8 string  |
| string16  | UTF-16 string                 |
| cstring16 | Null-terminated UTF-16 string |

#### Replacements
* `$length`
  - Length of array element (only available in plain arrays or extended pointer arrays)
* `$a`
  - Absolute offset of structure in stream
* `$i`
  - Index of array element (only available in array context)
* `$p` / `$parent`
  - Parent structure (for referencing parent members with '.')
* `$u` / `$unique`
  - Unique integer (sequential, for generating output IDs)
