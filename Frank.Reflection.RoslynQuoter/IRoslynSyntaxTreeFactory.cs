using Microsoft.CodeAnalysis;

using RoslynQuoter;

namespace Frank.Reflection.RoslynQuoter;

public interface IRoslynSyntaxTreeFactory
{
    SyntaxTree CreateSyntaxTree(string code);
    
    SyntaxNode? CreateSyntaxNode(string code, NodeKind nodeKind);
}