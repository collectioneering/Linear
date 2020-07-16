#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
java -Xmx500M org.antlr.v4.Tool -o $DIR/../src/Linear/Generated $DIR/../spec/Linear.g4 -Dlanguage=CSharp
