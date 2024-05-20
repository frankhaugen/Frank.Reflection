using System.Text.RegularExpressions;

using Frank.Reflection.Roslyn.Docs.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Frank.Reflection.Roslyn.Docs.Models.MethodInfo;

namespace Frank.Reflection.Roslyn.Docs;

public sealed class DocumentSyntaxAnalyzer
{
    public List<ClassInfo> Analyze(SyntaxTree? tree)
    {
        List<ClassInfo> result = new();

        List<ClassDeclarationSyntax> classNodes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Where(c => IsPublic(c.Modifiers)).ToList();
        foreach (ClassDeclarationSyntax node in classNodes)
        {
            result.Add(AnalyzeClass(node));
        }

        return result;
    }

    public static ClassInfo AnalyzeClass(ClassDeclarationSyntax node)
    {
        ClassInfo result = new();

        result.Name = GetFullName(node);
        result.Namespace = GetNamespace(node);
        result.Description = GetSummary(node);

        result.Methods = node.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Select(AnalyzeMethod)
            .ToList();

        result.Properties = node.DescendantNodes().OfType<PropertyDeclarationSyntax>()
            .Select(AnalyzeProperty)
            .ToList();

        result.Node = node;

        return result;
    }

    private static string GetFullName(ClassDeclarationSyntax node)
    {
        string result = node.Identifier.Text;

        SyntaxNode parent = node.Parent;
        while (true)
        {
            if (parent == null)
            {
                break;
            }

            ClassDeclarationSyntax? parentClass = parent as ClassDeclarationSyntax;
            if (parentClass == null)
            {
                break;
            }

            result = parentClass.Identifier.Text + "." + result;
            parent = parentClass.Parent;
        }

        return result;
    }

    private static string GetNamespace(ClassDeclarationSyntax node)
    {
        // see https://stackoverflow.com/questions/20458457/getting-class-fullname-including-namespace-from-roslyn-classdeclarationsyntax
        // used https://stackoverflow.com/a/23249021/2023653

        SyntaxNode parent = node.Parent;
        string result = null;

        while (true)
        {
            if (parent == null)
            {
                break;
            }

            if (parent.GetType() == typeof(NamespaceDeclarationSyntax))
            {
                result = (parent as NamespaceDeclarationSyntax).Name.GetText().ToString().Trim();
            }

            parent = parent.Parent;
        }

        return result;
    }

    private static PropertyInfo AnalyzeProperty(PropertyDeclarationSyntax node)
    {
        PropertyInfo result = new();

        result.Name = GetName(node.Identifier);
        result.Description = GetSummary(node);
        result.Category = GetCategory(node);
        result.OriginalTypeName = node.Type.ToString();
        result.Node = node;
        result.IsPublic = IsPublic(node.Modifiers);

        return result;
    }

    private static MethodInfo AnalyzeMethod(MethodDeclarationSyntax node)
    {
        MethodInfo result = new();

        result.Name = GetName(node.Identifier);
        result.Description = GetSummary(node);
        result.Category = GetCategory(node);
        result.OriginalTypeName = node.ReturnType.ToString();
        result.Parameters = node.ParameterList.Parameters.Select(AnalyzeParameter).ToList();
        result.Node = node;
        result.IsPublic = IsPublic(node.Modifiers);

        return result;
    }

    // help from https://stackoverflow.com/a/27675593/2023653
    private static string GetCategory(MemberDeclarationSyntax node)
    {
        AttributeSyntax[] attributes = node.AttributeLists.SelectMany(als => als.Attributes).ToArray();

        if (attributes.Any())
        {
            // find any [Category] attribute and return the argument from it,
            // for example [Category("helpers")] should return "helpers"
        }

        return null;
    }

    private static Parameter AnalyzeParameter(ParameterSyntax node)
    {
        Parameter result = new();
        result.OriginalTypeName = node.Type.ToString();
        result.Node = node;
        return result;
    }

    private static string? GetSummary(CSharpSyntaxNode node)
    {
        var xmlTrivia = node.GetLeadingTrivia()
            .Select(i => i.GetStructure())
            .OfType<DocumentationCommentTriviaSyntax>()
            .FirstOrDefault();

        var xmlComments = xmlTrivia?.ChildNodes()
            .OfType<XmlElementSyntax>()
            .ToList();

        if (xmlComments == null || !xmlComments.Any())
        {
            return null;
        }

        var elementSyntax = xmlComments.SkipWhile(x => !x.StartTag.Name.ToString().Equals("summary")).FirstOrDefault() ?? xmlComments.FirstOrDefault();
        if (elementSyntax == null)
        {
            return null;
        }

        string? description = elementSyntax.Content.ToFullString();
        description = Regex.Replace(description, @"\t*///", string.Empty).TrimStart('\r', '\n', ' ').TrimEnd('\r', '\n', ' ');

        return description;
    }

    private static string GetName(SyntaxToken syntaxToken)
    {
        return syntaxToken.Text;
    }

    private static bool IsPublic(SyntaxTokenList modifiers)
    {
        return modifiers.Any(m => m.Value != null && m.Value.Equals("public"));
    }
}