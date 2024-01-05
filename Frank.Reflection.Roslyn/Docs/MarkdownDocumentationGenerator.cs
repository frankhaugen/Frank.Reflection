using Frank.Markdown;
using Frank.Reflection.Roslyn.Docs.Extensions;
using Frank.Reflection.Roslyn.Docs.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.Reflection.Roslyn.Docs;

public class MarkdownDocumentationGenerator
{
    public IEnumerable<IMarkdownDocument> GenerateDocumentation(Solution solution) 
        => GetClassInfo(solution).Select(classinfo => classinfo.ToMarkdownDocument());

    public IEnumerable<ClassInfo> GetClassInfo(Solution solution) 
        => GetClassInfo(solution.Projects.SelectMany(x => x.Documents));
    
    public IEnumerable<ClassInfo> GetClassInfo(IEnumerable<Document> documents)
        => documents.SelectMany(document => GetClassInfoAsync(document).ToBlockingEnumerable());

    public async IAsyncEnumerable<ClassInfo> GetClassInfoAsync(Document document)
    {
        var tree = await document.GetSyntaxTreeAsync();;
        var root = await tree?.GetRootAsync();
        if (root == null)
            yield break;

        var types = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
        foreach (var type in types)
        {
            var classInfo = DocumentSyntaxAnalyzer.AnalyzeClass(type);
            
            yield return classInfo;
        }
    }
}