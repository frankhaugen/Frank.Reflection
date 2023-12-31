﻿using Frank.Reflection.Roslyn.Docs.Models;

namespace Frank.Reflection.Roslyn.Docs.Services;

public class CSharpMarkdownHelper
{
    /// <summary>
    ///     GitHub base path
    /// </summary>
    public string OnlinePath { get; set; }

    public string GetOnlineUrl(SourceLocation? location, bool withLineNumber = true)
    {
        string result = OnlinePath + location.Filename.Replace("\\", "/");

        if (withLineNumber)
        {
            result += "#L" + location.LineNumber;
        }

        return result;
    }

    public string TypeUrlOrName(IMemberInfo member)
    {
        string result = member.TypeLocation != null ? $"[{member.TypeName}]({GetOnlineUrl(member.TypeLocation)})" : member.OriginalTypeName;

        return EscapeBrackets(result);
    }

    public string GetMethodSignature(MethodInfo method)
    {
        return "(" + string.Join(", ", method.Parameters.Select(p => ArgText(p))) + ")";
    }

    internal string EscapeBrackets(string identifier)
    {
        string result = identifier;

        result = result.Replace("<", @"\<");
        result = result.Replace(">", @"\>");

        return result;
    }

    public string GetGenericArguments(MethodInfo method)
    {
        return method.HasGenericArguments() ? $"<{method.GetGenericArguments()}>" : string.Empty;
    }

    private string ArgText(MethodInfo.Parameter p)
    {
        string extension = p.IsExtension ? "this " : string.Empty;
        string paramArray = p.IsParams ? "params " : string.Empty;
        string optionalStart = p.IsOptional ? "[ " : string.Empty;
        string optionalEnd = p.IsOptional ? " ]" : string.Empty;

        string result = p.TypeLocation != null && !p.IsGeneric ? $"{optionalStart}{extension}{paramArray}[{p.OriginalTypeName}]({GetOnlineUrl(p.TypeLocation)}) {p.Name}{optionalEnd}" : $"{optionalStart}{extension}{paramArray}{p.OriginalTypeName} {p.Name}{optionalEnd}";

        return result;
    }
}