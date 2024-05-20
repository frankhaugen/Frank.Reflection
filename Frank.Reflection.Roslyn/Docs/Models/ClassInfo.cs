using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.Reflection.Roslyn.Docs.Models;

public class ClassInfo
{
    public ClassInfo()
    {
    }

    public ClassInfo(bool hasPartials)
    {
        // this is supposed to be so I can link to the right partial source file,
        // effectively overriding the Location property with a Locations property,
        // but I'm not sure yet how to make this work with the existing Methods and Properties members
        HasPartials = hasPartials;
    }

    public bool HasPartials { get; }

    public string? Namespace { get; set; }
    public string Name { get; set; }
    public bool IsStatic { get; set; }
    public string AssemblyName { get; set; }

    /// <summary>
    ///     XML summary comments
    /// </summary>
    public string? Description { get; set; }

    public SourceLocation? Location { get; set; }
    public ICollection<MethodInfo> Methods { get; set; }
    public ICollection<PropertyInfo> Properties { get; set; }

    public IEnumerable<string> Filenames { get; set; }
    public IEnumerable<ClassInfo> Partials { get; set; }

    public ClassDeclarationSyntax Node { get; set; }

    public IEnumerable<MethodInfo> GetMethods(string fileName = null)
    {
        if (!HasPartials)
        {
            return Methods;
        }

        Dictionary<string, ClassInfo> dictionary = Partials.ToDictionary(ci => ci.Location.Filename);
        return dictionary[fileName].Methods;
    }

    public IEnumerable<PropertyInfo> GetProperties(string fileName = null)
    {
        if (!HasPartials)
        {
            return Properties;
        }

        Dictionary<string, ClassInfo> dictionary = Partials.ToDictionary(ci => ci.Location.Filename);
        return dictionary[fileName].Properties;
    }

    internal static ClassInfo FromPartials(IEnumerable<ClassInfo> grp)
    {
        ClassInfo first = grp.First();

        return new ClassInfo(true)
        {
            Namespace = first.Namespace,
            Name = first.Name,
            IsStatic = first.IsStatic,
            AssemblyName = first.AssemblyName,
            Description = first.Description,
            Location = first.Location,
            Partials = grp,
            Filenames = grp.Select(ci => ci.Location.Filename)
        };
    }
}