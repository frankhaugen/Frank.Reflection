using Microsoft.CodeAnalysis;

namespace Frank.Reflection.RoslynQuoter;

public interface IRoslynSyntaxFactory
{
    Compilation CreateSyntaxFactoryCompilation(string code, string assemblyName);
}