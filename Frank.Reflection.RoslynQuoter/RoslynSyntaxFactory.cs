using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RoslynQuoter;

namespace Frank.Reflection.RoslynQuoter;

public class RoslynSyntaxFactory : IRoslynSyntaxFactory
{
    public Compilation CreateSyntaxFactoryCompilation(string code, string assemblyName)
    {
        var quoter = new Quoter();
        var quotedCode = quoter.QuoteText(code);
        var syntaxTree = SyntaxFactory.ParseSyntaxTree(quotedCode);
        var compilation = CSharpCompilation.Create(assemblyName)
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTree);
        
        return compilation;
    }
}