using Microsoft.CodeAnalysis; 
using Microsoft.CodeAnalysis.CSharp;

namespace RepoMind.Poc.Analysis;
public static class CSharpSourceParser
{
    public static ParseResult Parse(string sourceText, string filePath)
    {
        var tree = CSharpSyntaxTree.ParseText(sourceText, path: filePath);
        var diagnostics = tree.GetDiagnostics().ToList();

        return new ParseResult(tree, diagnostics);
    }
}
