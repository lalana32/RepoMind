using Microsoft.CodeAnalysis.CSharp;

namespace RepoMind.Poc.Analysis.Tests;

public class PlaceholderTests
{
    // Verifies the test project builds and sees Roslyn through the reference to RepoMind.Poc.Analysis.
    // Replace with real tests starting in RM-004.
    [Fact]
    public void RoslynIsAvailableThroughAnalysisLibrary()
    {
        Assert.NotNull(typeof(CSharpSyntaxTree));
    }
}
