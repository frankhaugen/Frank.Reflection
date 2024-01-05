using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Logging;

namespace Frank.Reflection.Roslyn.Helpers;

public class WorkspaceHelper
{
    private readonly ILogger<WorkspaceHelper> _logger;

    public WorkspaceHelper(ILogger<WorkspaceHelper> logger) => _logger = logger;

    public async Task<AdhocWorkspace> OpenAdhocWorkspaceAsync(FileInfo solutionFile)
    {
        var workspace = new AdhocWorkspace();
        workspace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId(Path.GetFileNameWithoutExtension(solutionFile.Name)), VersionStamp.Create(), solutionFile.FullName));

        var projectFiles = solutionFile.Directory?.EnumerateDirectories().Where(x => !x.Name.StartsWith(".")).SelectMany(x => x.EnumerateFiles("*.csproj"));
        workspace.WorkspaceFailed += OnWorkspaceFailed;

        var projects = projectFiles?.Select(x => ProjectInfo.Create(ProjectId.CreateNewId(Path.GetFileNameWithoutExtension(x.Name)), VersionStamp.Default, Path.GetFileNameWithoutExtension(x.FullName), Path.GetFileNameWithoutExtension(x.FullName), LanguageNames.CSharp, x.FullName));
        workspace.AddProjects(projects);

        return await Task.FromResult(workspace);
    }
    
    public async Task<MSBuildWorkspace> OpenMsBuildWorkspaceAsync(FileInfo solutionFile)
    {
        MSBuildLocator.RegisterDefaults();
        var workspace = MSBuildWorkspace.Create();
        workspace.WorkspaceFailed += OnWorkspaceFailed;
        
        await workspace.OpenSolutionAsync(solutionFile.FullName);
        return workspace;
    }

    private void OnWorkspaceFailed(object? sender, WorkspaceDiagnosticEventArgs e)
    {
        // _logger.LogError("Workspace failed: {DiagnosticKind} - {DiagnosticMessage}", e.Diagnostic.Kind, e.Diagnostic.Message);
    }
}