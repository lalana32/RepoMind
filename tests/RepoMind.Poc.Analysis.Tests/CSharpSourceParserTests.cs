namespace RepoMind.Poc.Analysis.Tests;

public class CSharpSourceParserTests
{
    [Fact]
    public void Parse_ValidSource_HasNoErrors()
    {
        // Given
        var source = """
            namespace Shop;

            public class OrderService
            {
                public void Place() { }
            }
            """;

        // When
        var result = CSharpSourceParser.Parse(source, "OrderService.cs");

        // Then
        Assert.False(result.HasErrors);
    }

    [Fact]
    public void Parse_MalformedSource_ReturnsTreeWithErrorDiagnostics()
    {
        // Given
        var source = """
            namespace Shop;

            public class Broken
            {
                public void Place() { }
            """;

        // When
        var result = CSharpSourceParser.Parse(source, "Broken.cs");

        // Then
        Assert.NotNull(result.Tree);
        Assert.True(result.HasErrors);
    }
}
