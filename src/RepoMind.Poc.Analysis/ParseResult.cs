using Microsoft.CodeAnalysis; 

namespace RepoMind.Poc.Analysis;
public sealed record ParseResult(SyntaxTree Tree, IReadOnlyList<Diagnostic> Diagnostics)
{
    public bool HasErrors => Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);
}
