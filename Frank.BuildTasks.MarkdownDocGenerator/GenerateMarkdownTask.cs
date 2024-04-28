using System.Reflection;

using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Frank.BuildTasks.MarkdownDocGenerator;

public class GenerateMarkdownTask : ITask
{
    private readonly ILogger<GenerateMarkdownTask> _logger;

    /// <summary>
    /// The directory containing the project to generate documentation for.
    /// </summary>
    [Required]
    public string ProjcectDirectory { get; set; }

    /// <summary>
    /// The directory to output the generated documentation to in markdown format.
    /// </summary>
    [Required]
    public string OutputDirectory { get; set; }

    static GenerateMarkdownTask()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
    }
    
    public GenerateMarkdownTask() : this(NullLogger<GenerateMarkdownTask>.Instance)
    {
    }

    public GenerateMarkdownTask(ILogger<GenerateMarkdownTask> logger)
    {
        _logger = logger;
    }

    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        if (args.Name.Contains("Microsoft.CodeAnalysis.CSharp"))
        {
            string assemblyPath = Path.Combine(Path.GetDirectoryName(typeof(GenerateMarkdownTask).Assembly.Location), "Microsoft.CodeAnalysis.CSharp.dll");
            if (File.Exists(assemblyPath))
                return Assembly.LoadFrom(assemblyPath);
        }
        return null;
    }

    public bool Execute()
    {
        CleanOutputDirectory();

        try
        {
            var fileSyntaxTreePairs = RoslynHelper.GetFileSyntaxTreePairs(ProjcectDirectory).ToList();
            
            var count = fileSyntaxTreePairs.Count();
            var outputDirectory = new DirectoryInfo(OutputDirectory);
            var projectDirectory = new DirectoryInfo(ProjcectDirectory);
            
            if (outputDirectory.Exists)
            {
                _logger.LogInformation("Output directory already exists, deleting contents");
                Console.WriteLine("Output directory already exists, deleting contents.");
                CleanOutputDirectory();
            }
            else
            {
                _logger.LogInformation("Output directory does not exist, creating");
                Console.WriteLine("Output directory does not exist, creating.");
                outputDirectory.Create();
            }
            
            _logger.LogInformation("Found {FileCount} files to generate documentation for", count);
            _logger.LogInformation("Outputting to {OutputDirectory}", outputDirectory.FullName);
            _logger.LogInformation("Project directory is {ProjectDirectory}", projectDirectory.FullName);
            
            Console.WriteLine($"Found {fileSyntaxTreePairs.Count()} files to generate documentation for.");
            Console.WriteLine($"Outputting to {outputDirectory.FullName}");
            Console.WriteLine($"Project directory is {projectDirectory.FullName}");

            MarkdownHelper.GenerateMarkdownDocumentation(fileSyntaxTreePairs, outputDirectory);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating markdown documentation");
            Console.Error.WriteLine(ex);
            return false;
        }
    }

    private void CleanOutputDirectory()
    {
        var directory = new DirectoryInfo(OutputDirectory);
        if (directory.Exists)
        {
            var files = directory.GetFiles();
            foreach (var file in files)
            {
                _logger.LogInformation("Deleting {FileFullName}", file.FullName);
                Console.WriteLine($"Deleting {file.FullName}");
                file.Delete();
            }
            
            var directories = directory.GetDirectories();
            foreach (var dir in directories)
            {
                _logger.LogInformation("Deleting {DirectoryFullName}", dir.FullName);
                Console.WriteLine($"Deleting {dir.FullName}");
                dir.Delete(true);
            }
        }
    }

    /// <inheritdoc />
    public IBuildEngine BuildEngine { get; set; }

    /// <inheritdoc />
    public ITaskHost HostObject { get; set; }
}