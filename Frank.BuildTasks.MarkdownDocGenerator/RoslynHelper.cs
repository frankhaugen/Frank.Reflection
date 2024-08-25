using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.BuildTasks.MarkdownDocGenerator;

public static class RoslynHelper
{
    public static IEnumerable<KeyValuePair<FileInfo, SyntaxTree>> GetFileSyntaxTreePairs(string directory) =>
        DirectoryPathHelper
            .GetCsFiles(directory)
            .Select(file => new KeyValuePair<FileInfo, SyntaxTree> (
                file, 
                SyntaxFactory.ParseSyntaxTree(File.ReadAllText(file.FullName))
                ));

    public static Dictionary<string, string?> GetXmlDocsParts(MemberDeclarationSyntax memberDeclarationSyntax)
    {
        var xmlTrivia = memberDeclarationSyntax.GetLeadingTrivia()
            .FirstOrDefault(trivia => trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));
        
        if (xmlTrivia == default) return new Dictionary<string, string?>();

        var xmlDoc = xmlTrivia.ToString();
        
        var xmlDocParts = new Dictionary<string, string?>()
        {
            {"Summary", null},
            {"Remarks", null},
            {"Example", null},
            {"Returns", null},
            {"Value", null},
            {"Exception", null},
            {"Permission", null},
            {"See", null},
            {"SeeAlso", null},
            {"Include", null}
        };
        
        foreach (var key in xmlDocParts.Keys)
        {
            var startTag = $"<{key}>";
            var endTag = $"</{key}>";
            var startIndex = xmlDoc.IndexOf(startTag, StringComparison.OrdinalIgnoreCase);
            var endIndex = xmlDoc.IndexOf(endTag, StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1 || endIndex == -1) continue;
            xmlDocParts[key] = xmlDoc.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length);
        }

        return xmlDocParts;
    }
}