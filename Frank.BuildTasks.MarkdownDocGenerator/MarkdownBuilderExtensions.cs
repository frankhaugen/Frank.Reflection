namespace Frank.BuildTasks.MarkdownDocGenerator;

public static class MarkdownBuilderExtensions
{
    public static MarkdownBuilder AppendHeader(this MarkdownBuilder builder, string header, int level = 1) => builder.AppendLine($"{new string('#', level)} {header}");

    public static MarkdownBuilder AppendCodeBlock(this MarkdownBuilder builder, string code) => builder.AppendLine("```csharp").AppendLine(code).AppendLine("```");
    
    public static MarkdownBuilder AppendLink(this MarkdownBuilder builder, string text, string url) => builder.Append($"[{text}]({url})");

    public static MarkdownBuilder AppendBold(this MarkdownBuilder builder, string text) => builder.Append($"**{text}**");

    public static MarkdownBuilder AppendItalic(this MarkdownBuilder builder, string text) => builder.Append($"*{text}*");

    public static MarkdownBuilder AppendStrikethrough(this MarkdownBuilder builder, string text) => builder.Append($"~~{text}~~");

    public static MarkdownBuilder AppendQuote(this MarkdownBuilder builder, string text) => builder.AppendLine($"> {text}");

    public static MarkdownBuilder AppendHorizontalRule(this MarkdownBuilder builder) => builder.AppendLine("---");

    public static MarkdownBuilder AppendImage(this MarkdownBuilder builder, string altText, string imageUrl) => builder.AppendLine($"![{altText}]({imageUrl})");

    public static MarkdownBuilder AppendLinkImage(this MarkdownBuilder builder, string altText, string imageUrl, string linkUrl) => builder.AppendLine($"[![{altText}]({imageUrl})]({linkUrl})");
    
    public static MarkdownBuilder AppendInlineCode(this MarkdownBuilder builder, string code) => builder.Append($"`{code}`");

    public static MarkdownBuilder AppendCodeBlock(this MarkdownBuilder builder, string code, string language) => builder.AppendLine($"```{language}").AppendLine(code).AppendLine("```");

    public static MarkdownBuilder AppendTableOfContents(this MarkdownBuilder builder, IEnumerable<string> headers)
    {
        foreach (var header in headers) builder.AppendLine($"- [{header}](#{header.ToLower().Replace(" ", "-")})");
        return builder;
    }
    
    public static MarkdownBuilder AppendTaskList(this MarkdownBuilder builder, IEnumerable<(string, bool)> items)
    {
        foreach (var (item, done) in items) builder.AppendLine($"- [{(done ? "x" : " ")}] {item}");
        return builder;
    }

    public static MarkdownBuilder AppendList(this MarkdownBuilder builder, IEnumerable<string> items)
    {
        foreach (var item in items) builder.AppendLine($"- {item}");
        return builder;
    }
    
    public static MarkdownBuilder AppendOrderedList(this MarkdownBuilder builder, IEnumerable<string> items)
    {
        int i = 1;
        foreach (var item in items) builder.AppendLine($"{i++}. {item}");
        return builder;
    }
    
    public static MarkdownBuilder AppendTable<T>(this MarkdownBuilder builder, IEnumerable<T> source)
    {
        var properties = typeof(T).GetProperties();
        var headers = properties.Select(x => x.Name);
        var rows = source.Select(x => properties.Select(p => p.GetValue(x).ToString()));
        return builder.AppendTable(headers, rows);
    }
    
    public static MarkdownBuilder AppendTable(this MarkdownBuilder builder, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows)
    {
        builder.AppendLine("|" + string.Join("|", headers) + "|");
        builder.AppendLine("|" + string.Join("|", headers.Select(_ => "---")) + "|");
        foreach (var row in rows) builder.AppendLine("|" + string.Join("|", row) + "|");
        return builder;
    }
}
