namespace RepoMind.Poc.Analysis;

public sealed record TypeDeclarationFact(
    string Name,
    TypeDeclarationKind Kind,
    string? Namespace,
    string? ContainingType,
    IReadOnlyList<string> Modifiers,
    string FilePath,
    int Line,
    int Column,
    FactOrigin Origin)
{
    public bool IsPartial => Modifiers.Contains("partial");
}
