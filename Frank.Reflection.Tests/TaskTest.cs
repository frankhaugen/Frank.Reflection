using Frank.BuildTasks.MarkdownDocGenerator;
using Frank.Testing.Logging;

using Xunit.Abstractions;

namespace Frank.Reflection.Tests;

public class TaskTest
{
    private readonly ITestOutputHelper _outputHelper;

    public TaskTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void Test1()
    {
        var fff = new GenerateMarkdownTask(_outputHelper.CreateTestLogger<GenerateMarkdownTask>())
        {
            OutputDirectory = "../../../DocsTest",
            ProjectDirectory = "../../../Frank.Reflection"
        };
        fff.Execute();
    }
}