using System.Text;

using Microsoft.CodeAnalysis;

namespace Frank.BuildTasks.MarkdownDocGenerator;

public static class MarkdownHelper
{
    public static void GenerateMarkdownDocumentation(IEnumerable<KeyValuePair<FileInfo,SyntaxTree>> fileSyntaxTreePairs, DirectoryInfo outputDirectory)
    {
        if (!outputDirectory.Exists) outputDirectory.Create();
        
        var outputFileSyntaxTreePairs = fileSyntaxTreePairs
            .Select(pair => new KeyValuePair<FileInfo, SyntaxTree>(
                CreateOutputFile(pair.Key, outputDirectory),
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

    private static FileInfo CreateOutputFile(FileInfo pairKey, DirectoryInfo outputDirectory)
    {
        var commonRoot = pairKey.Directory!.FullName.Replace(outputDirectory.FullName, "");
        var outputFilePath = Path.Combine(outputDirectory.FullName, commonRoot, Path.ChangeExtension(pairKey.Name, ".md"));
        var outputFile = new FileInfo(outputFilePath);
        if (!outputFile.Directory?.Exists ?? false) outputFile.Directory.Create();
        return outputFile;
    }

    private static string GenerateMarkdownDocumentation(KeyValuePair<FileInfo, SyntaxTree> fileSyntaxTreePairs)
    {
        StringBuilder sb = new();
        
        sb.AppendLine($"# {fileSyntaxTreePairs.Key.Name}");
        
        return sb.ToString();
    }
}