using System.Text;

namespace Frank.BuildTasks.MarkdownDocGenerator;

public class MarkdownBuilder
{
    private readonly StringBuilder _sb = new();
    
    public MarkdownBuilder AppendLine(string line)
    {
        _sb.AppendLine(line);
        return this;
    }
    
    public MarkdownBuilder Append(string line)
    {
        _sb.Append(line);
        return this;
    }
    
    public override string ToString() => _sb.ToString();
}