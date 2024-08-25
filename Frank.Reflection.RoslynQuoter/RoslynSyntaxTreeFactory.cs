using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RoslynQuoter;

namespace Frank.Reflection.RoslynQuoter;

public class RoslynSyntaxTreeFactory : IRoslynSyntaxTreeFactory
{
    public SyntaxTree CreateSyntaxTree(string code) => SyntaxFactory.ParseSyntaxTree(code);

    public SyntaxNode? CreateSyntaxNode(string code, NodeKind nodeKind)
    {
        var parseOptions = new CSharpParseOptions(LanguageVersion.Preview, DocumentationMode.Diagnose);
        
        return nodeKind switch
        {
            NodeKind.Script => SyntaxFactory.ParseCompilationUnit(code, 0, parseOptions.WithKind(SourceCodeKind.Script)),
            NodeKind.CompilationUnit => SyntaxFactory.ParseCompilationUnit(code, 0, parseOptions),
            NodeKind.MemberDeclaration => SyntaxFactory.ParseMemberDeclaration(code, 0, parseOptions),
            NodeKind.Statement => SyntaxFactory.ParseStatement(code, 0, parseOptions),
            NodeKind.Expression => SyntaxFactory.ParseExpression(code, 0, parseOptions),
            _ => throw new InvalidOperationException("Invalid NodeKind")
        };
    }
}