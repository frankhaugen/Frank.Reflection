using Frank.Reflection.Roslyn.Docs.Models;

using Microsoft.CodeAnalysis;

namespace Frank.Reflection.Roslyn.Docs;

public sealed class SolutionAnalyzer
{
    private readonly DocumentSemanticAnalyzer _semanticAnalyzer = new();
    private readonly DocumentSyntaxAnalyzer _syntaxAnalyzer = new();

    public async Task<IEnumerable<ClassInfo>> AnalyzeAsync(Solution solution, string solutionPath)
    {
        List<ClassInfo> result = new();

        foreach (var doc in solution.Projects.SelectMany(p => p.Documents))
        {
            // Syntax Info
            var tree = await doc.GetSyntaxTreeAsync();
            var classInfos = _syntaxAnalyzer.Analyze(tree);

            // Semantic Info
            var model = await doc.GetSemanticModelAsync();
            await _semanticAnalyzer.Analyze(solution, model, classInfos);

            result.AddRange(classInfos);
        }

        SetRelativePaths(solutionPath, result);

        return result;
    }

    private static void SetRelativePaths(string solutionPath, IEnumerable<ClassInfo> classInfos)
    {
        string? basePath = Path.GetDirectoryName(solutionPath);

        // turns out we need to remove the last folder name for GitHub link compatibility
        //var folders = basePath.Split('\\');
        //basePath = string.Join("\\", folders.Take(folders.Length - 1));

        foreach (ClassInfo classInfo in classInfos)
        {
            SetRelativePath(classInfo.Location, basePath);
            foreach (MethodInfo methodInfo in classInfo.Methods)
            {
                SetRelativePath(methodInfo.Location, basePath);
                SetRelativePath(methodInfo.TypeLocation, basePath);
                SetReferenceRelativePaths(methodInfo, basePath);

                foreach (MethodInfo.Parameter p in methodInfo.Parameters.Where(p => p.TypeLocation != null))
                {
                    SetRelativePath(p.TypeLocation, basePath);
                }
            }

            foreach (PropertyInfo propertyInfo in classInfo.Properties)
            {
                SetRelativePath(propertyInfo.Location, basePath);
                SetRelativePath(propertyInfo.TypeLocation, basePath);
                SetReferenceRelativePaths(propertyInfo, basePath);
            }
        }
    }

    private static void SetReferenceRelativePaths(IMemberInfo member, string? basePath)
    {
        foreach (SourceLocation? reference in member.References)
        {
            SetRelativePath(reference, basePath);
        }
    }

    private static void SetRelativePath(SourceLocation? location, string? basePath)
    {
        if (location == null || string.IsNullOrWhiteSpace(location.Filename))
        {
            return;
        }

        if (basePath != null)
        {
            location.Filename = location.Filename.Substring(basePath.Length + 1);
        }
    }
}