using System.Collections.Generic;
using Linear.Runtime;
using Linear.Runtime.Expressions;
using NUnit.Framework;

namespace Linear.Test;

public class TestsBase
{
    public Dictionary<string, DeserializerDefinition> Deserializers;
    public Dictionary<string, MethodCallDelegate> Methods;

    [SetUp]
    public void SetUp()
    {
        Deserializers = new Dictionary<string, DeserializerDefinition>();
        Methods = new Dictionary<string, MethodCallDelegate>();
    }

    public FormatStore Create(params string[] files)
    {
        var res = new FormatStore(Deserializers, Methods);
        foreach (string file in files)
            res.Add(file);
        return res;
    }

    public void AddBufferMethod(string name, byte[] buffer)
    {
        Methods.Add(name, (_, _) => buffer);
    }
}
