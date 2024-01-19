using Frank.Reflection.Roslyn.Docs;

using JetBrains.Annotations;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;

using Xunit;

using System.Linq;

using FluentAssertions;

using Frank.Reflection.Roslyn.Extensions;
using Frank.Reflection.Roslyn.Helpers;
using Frank.Testing.Logging;

using Xunit.Abstractions;

namespace Frank.Reflection.Tests.Docs;

[TestSubject(typeof(MarkdownDocumentationGenerator))]
public class MarkdownDocumentationGeneratorTests
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly MarkdownDocumentationGenerator _markdownDocumentationGenerator = new();
    private readonly FileInfo _solutionFile = new(Path.Combine(AppContext.BaseDirectory, "../../../../Frank.Reflection.sln"));

    public MarkdownDocumentationGeneratorTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    // [Fact]
    public async Task GenerateDocumentationForType_ReturnsCorrectDocumentationForSimpleType()
    {
        // Arrange
        var workspaceHelper = new WorkspaceHelper(_outputHelper.CreateTestLogger<WorkspaceHelper>());
        var workspace = await workspaceHelper.OpenMsBuildWorkspaceAsync(_solutionFile);
        var solution = workspace.CurrentSolution;
        
        // Act
        var result = _markdownDocumentationGenerator.GetClassInfo(solution);

        // Assert
        var markdownDocuments = _markdownDocumentationGenerator.GenerateDocumentation(solution);
        _outputHelper.WriteLine(string.Join("\n", markdownDocuments.Select(x => x.ToString())));
        _outputHelper.WriteLine(result.Select(x => x.Name + " " + x.Description));
        result.Should().NotBeEmpty();
    }
}