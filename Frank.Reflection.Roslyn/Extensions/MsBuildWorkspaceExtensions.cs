using Frank.Markdown;
using Frank.Reflection.Roslyn.Docs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.Reflection.Roslyn.Extensions;

public class MsBuildWorkspaceExtensions
{
    private readonly MarkdownDocumentationGenerator _markdownDocumentationGenerator = new();
    
    public IEnumerable<IMarkdownDocument> GenerateDocumentation(Solution solution) 
        => _markdownDocumentationGenerator.GenerateDocumentation(solution);

    public async IAsyncEnumerable<KeyValuePair<FileInfo, TypeDeclarationSyntax>> GetTypeSyntaxAsync(Solution solution)
    {
        foreach (var project in solution.Projects)
        {
            foreach (var document in project.Documents)
            {
                var syntaxTree = await document.GetSyntaxTreeAsync();
                var root = await syntaxTree?.GetRootAsync()!;
                var types = root?.DescendantNodes().OfType<TypeDeclarationSyntax>();
                if (types == null)
                    continue;
                
                foreach (var type in types)
                    yield return new KeyValuePair<FileInfo, TypeDeclarationSyntax>(new FileInfo(document.FilePath!), type);
            }
        }
    }
}