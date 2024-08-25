using FluentAssertions;

using Frank.Reflection.RoslynQuoter;

using JetBrains.Annotations;

namespace Frank.Reflection.Tests.RoslynQuoter;

[TestSubject(typeof(RoslynSyntaxTreeFactory))]
public class RoslynSyntaxTreeFactoryTests
{
    private readonly ITestOutputHelper _outputHelper;

    public RoslynSyntaxTreeFactoryTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void Test()
    {
        var quoter = new RoslynSyntaxTreeFactory();
        var code = """
                   using System;
                   namespace Frank.Reflection.Tests
                   {
                     public class Test
                     {
                       public void Method()
                       {
                            Console.WriteLine(""Hello, World!"");
                       }
                     }
                   }
                   """;
        var tree = quoter.CreateSyntaxTree(code);
        tree.Should().NotBeNull();

        var quotedCode = tree.ToString();
        
        _outputHelper.WriteLine(quotedCode);
    }
}