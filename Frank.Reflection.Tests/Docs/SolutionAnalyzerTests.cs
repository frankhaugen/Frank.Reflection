using Frank.Reflection.Roslyn.Docs;

using Microsoft.CodeAnalysis;

using FluentAssertions;

using JetBrains.Annotations;

using Microsoft.CodeAnalysis.MSBuild;

using Xunit.Abstractions;


namespace Frank.Reflection.Tests.Docs
{
    [TestSubject(typeof(SolutionAnalyzer))]
    public class SolutionAnalyzerTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly SolutionAnalyzer _solutionAnalyzer = new();
        private readonly FileInfo _solutionFile = new(Path.Combine(AppContext.BaseDirectory, "../../../../Frank.Reflection.sln"));

        public SolutionAnalyzerTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task Analyze_Should_Not_Return_Null()
        {
            // Arrange
            var solution = await GetSolution();

            // Act
            var result = await _solutionAnalyzer.AnalyzeAsync(solution, string.Empty);

            // Assert
            result.Should().NotBeNull();
            _outputHelper.WriteLine(result.Select(x => x.Name));
        }

        private async Task<Solution> GetSolution()
        {
            var workspace = await GetMsBuildWorkspaceFromSolution(_solutionFile);
            workspace.LoadMetadataForReferencedProjects = true;

            var solution = workspace.CurrentSolution;
            return solution;
        }

        [Fact]
        public async Task Analyze_Should_Return_Items_For_Each_Document()
        {
            // Arrange
            var solution = await GetSolution();

            // Act
            var result = _solutionAnalyzer.AnalyzeAsync(solution, string.Empty).Result;

            // Assert
            result.Should().HaveCountGreaterThan(1);
        }
        
        async Task<MSBuildWorkspace> GetMsBuildWorkspaceFromSolution(FileInfo solutionFile)
        {
            var workspace = MSBuildWorkspace.Create();

            await workspace.OpenSolutionAsync(solutionFile.FullName);

            return workspace;
        }
    }
}