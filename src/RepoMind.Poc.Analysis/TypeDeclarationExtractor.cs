using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RepoMind.Poc.Analysis;

// Extracts class and interface declarations from a single syntax tree.
//
// Everything here is a syntax fact: it describes what is written in this file only.
// Each part of a partial class is reported as its own fact, because syntax alone cannot
// prove that two parts belong to the same type. Merging them requires a SemanticModel.
//
// Records, structs and enums are deliberately ignored for the PoC.
public static class TypeDeclarationExtractor
{
    public static IReadOnlyList<TypeDeclarationFact> Extract(SyntaxTree syntaxTree)
    {
        var root = syntaxTree.GetRoot();
        var facts = new List<TypeDeclarationFact>();

        foreach (var typeNode in root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            // RecordDeclarationSyntax and StructDeclarationSyntax also derive from
            // TypeDeclarationSyntax, so filter explicitly instead of relying on the base type.
            TypeDeclarationKind? kind = typeNode switch
            {
                ClassDeclarationSyntax => TypeDeclarationKind.Class,
                InterfaceDeclarationSyntax => TypeDeclarationKind.Interface,
                _ => null
            };

            if (kind is null)
            {
                continue;
            }

            // Roslyn line/column are 0-based; RepoMind reports 1-based like editors do.
            // The location points at the type name, not at attributes or modifiers.
            var position = typeNode.Identifier.GetLocation().GetLineSpan().StartLinePosition;

            facts.Add(new TypeDeclarationFact(
                typeNode.Identifier.Text,
                kind.Value,
                GetNamespace(typeNode),
                GetContainingType(typeNode),
                typeNode.Modifiers.Select(modifier => modifier.Text).ToList(),
                syntaxTree.FilePath,
                position.Line + 1,
                position.Character + 1,
                FactOrigin.Syntax));
        }

        return facts;
    }

    // Nested namespace blocks are joined from outermost to innermost, e.g. "Shop.Orders".
    private static string? GetNamespace(SyntaxNode node)
    {
        var names = node.Ancestors()
            .OfType<BaseNamespaceDeclarationSyntax>()
            .Select(ns => ns.Name.ToString())
            .Reverse()
            .ToList();

        return names.Count == 0 ? null : string.Join(".", names);
    }

    // For deeply nested types the full chain is returned, e.g. "Outer.Middle".
    private static string? GetContainingType(SyntaxNode node)
    {
        var names = node.Ancestors()
            .OfType<BaseTypeDeclarationSyntax>()
            .Select(type => type.Identifier.Text)
            .Reverse()
            .ToList();

        return names.Count == 0 ? null : string.Join(".", names);
    }
}
