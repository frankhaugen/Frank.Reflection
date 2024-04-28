namespace Frank.BuildTasks.MarkdownDocGenerator;

public static class DirectoryPathHelper
{
    public static IEnumerable<FileInfo> GetCsFiles(string directory) =>
        new DirectoryInfo(directory).GetFiles("*.cs", SearchOption.AllDirectories);
}