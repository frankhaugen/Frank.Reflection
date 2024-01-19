using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.Reflection.Dump;

public static class DumpExtensions
{
    public static string DumpVar<T>(this T obj, VarDump.Visitor.DumpOptions? options = null) => VariableFactory.DumpVar(obj, options);
    
    public static string DumpClass<T>(this T obj, VarDump.Visitor.DumpOptions? options = null) => ClassFactory.CreateClass(obj, options);

    public static string DumpEnumerable<T>(this IEnumerable<T> objs, Func<T, string> idSelector, VarDump.Visitor.DumpOptions? options = null) => ClassFactory.CreateEnumerableClass(objs, idSelector, options);

    public static ClassDeclarationSyntax DumpClassDeclarationSyntax<T>(T obj, VarDump.Visitor.DumpOptions? options = null)
        => SyntaxFactory.ParseSyntaxTree(DumpClass(obj, options)).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First().NormalizeWhitespace();
    
    public static MethodDeclarationSyntax DumpMethodDeclarationSyntax<T>(T obj, VarDump.Visitor.DumpOptions? options = null)
        => AsMethodDeclarationSyntax(SyntaxFactory.ParseMemberDeclaration(MethodFactory.CreateMethod(obj, options)) ?? throw new Exception("Could not parse method declaration syntax.")).NormalizeWhitespace();

    private static MethodDeclarationSyntax AsMethodDeclarationSyntax(MemberDeclarationSyntax memberDeclarationSyntax)
        => memberDeclarationSyntax as MethodDeclarationSyntax ?? throw new Exception("Could not parse method declaration syntax.");
}