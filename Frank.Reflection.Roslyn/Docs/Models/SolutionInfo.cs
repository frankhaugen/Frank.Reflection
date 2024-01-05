using System.Text.Json.Serialization;

namespace Frank.Reflection.Roslyn.Docs.Models;

public class SolutionInfo
{
    public DateTime Timestamp { get; set; }
    public string RepoUrl { get; set; }
    public string BranchName { get; set; }

    public List<ClassInfo> Classes { get; set; }

    [JsonIgnore]
    public Dictionary<string, ClassInfo> ClassDictionary => Classes
        .GroupBy(c => $"{c.Namespace}.{c.Name}")
        .ToDictionary(grp => grp.Key, grp => ClassInfo.FromPartials(grp));

    public string SourceFileUrl(string path)
    {
        return $"{SourceFileBase()}{path}";
    }

    public string SourceFileBase()
    {
        return $"{RepoUrl}/blob/{BranchName}/";
    }
}