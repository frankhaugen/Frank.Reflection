using System.Reflection;
using System.Text;

namespace Frank.Reflection.Mermaid;

public class ClassDiagramBuilder : IClassDiagramBuilder
{
    private readonly Assembly _assembly;

    public ClassDiagramBuilder(Assembly assembly)
    {
        _assembly = assembly;
    }

    public string Build()
    {
        var types = _assembly.GetTypes();
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("classDiagram");
        foreach (var type in types)
        {
            stringBuilder.AppendLine($"class {type.GetFriendlyName()}");
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                stringBuilder.AppendLine($"    {constructor.GetDisplayName()}");
            }
        }

        return stringBuilder.ToString();
    }
}

public interface IClassDiagramBuilder : IDiagramBuilder
{
    
}

public interface IDiagramBuilder
{
    public string Build();
}