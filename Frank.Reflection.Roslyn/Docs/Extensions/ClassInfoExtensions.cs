using Frank.Markdown;
using Frank.Reflection.Roslyn.Docs.Models;

namespace Frank.Reflection.Roslyn.Docs.Extensions;

public static class ClassInfoExtensions
{
    public static IMarkdownDocument ToMarkdownDocument(this ClassInfo classInfo)
    {
        var document = new MarkdownDocument();
        document.WithHeader(classInfo.Name, 1);
        document.WithParagraph(new MarkdownParagraph().WithText(classInfo.Description));
        document.WithHeader("Methods", 2);
        foreach (var method in classInfo.Methods)
        {
            document.WithHeader(method.Name, 3);
            document.WithParagraph(new MarkdownParagraph().WithText(method.Description));
            document.WithHeader("Parameters", 4);
            foreach (var parameter in method.Parameters)
            {
                document.WithHeader(parameter.Name, 5);
                document.WithParagraph(new MarkdownParagraph().WithText(parameter.Name));
            }
        }
        document.WithHeader("Properties", 2);
        foreach (var property in classInfo.Properties)
        {
            document.WithHeader(property.Name, 3);
            document.WithParagraph(new MarkdownParagraph().WithText(property.Description));
        }
        return document;
    }
}