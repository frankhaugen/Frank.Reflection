using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using XmlDocMarkdown.Core;

namespace Frank.BuildTasks.MarkdownDocGenerator;

public static class MarkdownHelper
{
    public static void GenerateMarkdownDocumentation(IEnumerable<KeyValuePair<FileInfo,SyntaxTree>> fileSyntaxTreePairs, DirectoryInfo outputDirectory)
    {
        if (!outputDirectory.Exists) outputDirectory.Create();
        
        var outputFileSyntaxTreePairs = fileSyntaxTreePairs
            .Select(pair => new KeyValuePair<FileInfo, SyntaxTree>(
                CreateOutputFile(pair.Key, GetNamspace(pair.Value), outputDirectory),
                pair.Value
            ));
        
        var outputFilesMarkdownPairs = outputFileSyntaxTreePairs
            .Select(pair => new KeyValuePair<FileInfo, string>(
                pair.Key,
                GenerateMarkdownDocumentation(pair)
            )).ToList();
        
        foreach (var pair in outputFilesMarkdownPairs)
        {
            Console.WriteLine($"Generating markdown for {pair.Key.FullName}");
            File.WriteAllText(pair.Key.FullName, pair.Value);
        }
        
        Console.WriteLine($"Generated {outputFilesMarkdownPairs.Count()} markdown files.");
    }

    private static IEnumerable<string> GetNamspace(SyntaxTree pairValue)
    {
        var root = pairValue.GetRoot();
        var @namespace = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().SelectMany(n => n.Name.ToString().Split('.'));
        return @namespace;
    }

    private static FileInfo CreateOutputFile(FileInfo pairKey, IEnumerable<string> @namespace, DirectoryInfo outputDirectory)
    {
        var fileName = Path.ChangeExtension(pairKey.Name, ".md");
        var directory = Path.Combine(outputDirectory.FullName, string.Join("/", @namespace), fileName);
        var fileInfo = new FileInfo(directory);
        if (!fileInfo.Directory!.Exists) fileInfo.Directory.Create();
        return fileInfo;
    }

    private static string GenerateMarkdownDocumentation(KeyValuePair<FileInfo, SyntaxTree> fileSyntaxTreePairs)
    {
        var markdownBuilder = new MarkdownBuilder();

        var ss =new XmlDocInput()
        {
            Assembly = Assembly.LoadFrom(fileSyntaxTreePairs.Key.FullName),
        };
            
        return markdownBuilder.ToString();
    }
}