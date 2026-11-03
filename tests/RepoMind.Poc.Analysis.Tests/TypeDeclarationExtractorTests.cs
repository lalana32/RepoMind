namespace RepoMind.Poc.Analysis.Tests;

public class TypeDeclarationExtractorTests
{
    [Fact]
    public void Extract_ClassInBlockScopedNamespace_ReturnsClassWithNamespace()
    {
        // Given
        var source = """
            namespace Shop
            {
                public class Order { }
            }
            """;
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal("Order", fact.Name);
        Assert.Equal(TypeDeclarationKind.Class, fact.Kind);
        Assert.Equal("Shop", fact.Namespace);
    }

    [Fact]
    public void Extract_ClassInFileScopedNamespace_ReturnsClassWithNamespace()
    {
        // Given
        var source = """
            namespace Shop;

            public class Order { }
            """;
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal("Order", fact.Name);
        Assert.Equal(TypeDeclarationKind.Class, fact.Kind);
        Assert.Equal("Shop", fact.Namespace);
    }

    [Fact]
    public void Extract_ClassInNestedNamespaces_ReturnsFullNamespace()
    {
        // Given
        var source = """
            namespace Shop
            {
                namespace Orders
                {
                    public class Order { }
                }
            }
            """;
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal("Shop.Orders", fact.Namespace);
    }

    [Fact]
    public void Extract_InterfaceInBlockScopedNamespace_ReturnsInterfaceWithNamespace()
    {
        // Given
        var source = """
            namespace Shop
            {
                public interface IOrderRepository { }
            }
            """;
        var result = CSharpSourceParser.Parse(source, "IOrderRepository.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal("IOrderRepository", fact.Name);
        Assert.Equal(TypeDeclarationKind.Interface, fact.Kind);
        Assert.Equal("Shop", fact.Namespace);
    }

    [Fact]
    public void Extract_InterfaceInFileScopedNamespace_ReturnsInterfaceWithNamespace()
    {
        // Given
        var source = """
            namespace Shop;

            public interface IOrderRepository { }
            """;
        var result = CSharpSourceParser.Parse(source, "IOrderRepository.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal("IOrderRepository", fact.Name);
        Assert.Equal(TypeDeclarationKind.Interface, fact.Kind);
        Assert.Equal("Shop", fact.Namespace);
    }

    [Fact]
    public void Extract_TypeWithoutNamespace_ReturnsNullNamespace()
    {
        // Given
        var source = "public class Order { }";
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Null(fact.Namespace);
        Assert.Null(fact.ContainingType);
    }

    [Fact]
    public void Extract_NestedTypes_ReportContainingType()
    {
        // Given
        var source = """
            namespace Shop;

            public class Order
            {
                private class Line
                {
                    public interface IPriced { }
                }
            }
            """;
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        Assert.Equal(3, facts.Count);
        Assert.Null(facts.Single(f => f.Name == "Order").ContainingType);
        Assert.Equal("Order", facts.Single(f => f.Name == "Line").ContainingType);
        Assert.Equal("Order.Line", facts.Single(f => f.Name == "IPriced").ContainingType);
        Assert.All(facts, f => Assert.Equal("Shop", f.Namespace));
    }

    [Fact]
    public void Extract_PartialClassParts_ReportsEachPartSeparately()
    {
        // Syntax alone cannot prove that both parts are the same type, so they are
        // reported as two facts. Merging them is a semantic (SemanticModel) concern.

        // Given
        var source = """
            namespace Shop;

            public partial class Order { }

            partial class Order { }
            """;
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        Assert.Equal(2, facts.Count);
        Assert.All(facts, f => Assert.Equal("Order", f.Name));
        Assert.All(facts, f => Assert.True(f.IsPartial));
        Assert.Equal(["public", "partial"], facts[0].Modifiers);
        Assert.Equal(["partial"], facts[1].Modifiers);
        Assert.NotEqual(facts[0].Line, facts[1].Line);
    }

    [Fact]
    public void Extract_Modifiers_AreReportedInSourceOrder()
    {
        // Given
        var source = "internal abstract class Order { }";
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal(["internal", "abstract"], fact.Modifiers);
        Assert.False(fact.IsPartial);
    }

    [Fact]
    public void Extract_Location_IsOneBasedAndPointsAtTypeName()
    {
        // Given
        // Line 3, "Order" starts after "    public class " (17 chars) => column 18.
        var source = """
            namespace Shop
            {
                public class Order { }
            }
            """;
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal("Order.cs", fact.FilePath);
        Assert.Equal(3, fact.Line);
        Assert.Equal(18, fact.Column);
    }

    [Fact]
    public void Extract_TypeOnFirstLine_ReportsLineAndColumnOne()
    {
        // Given
        var source = "class Order { }";
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal(1, fact.Line);
        Assert.Equal(7, fact.Column);
    }

    [Fact]
    public void Extract_RecordsStructsAndEnums_AreNotReported()
    {
        // Given
        var source = """
            namespace Shop;

            public record OrderPlaced(int Id);
            public record class OrderShipped(int Id);
            public record struct Money(decimal Amount);
            public struct Point { }
            public enum OrderStatus { New, Paid }
            public class Order { }
            """;
        var result = CSharpSourceParser.Parse(source, "Types.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        var fact = Assert.Single(facts);
        Assert.Equal("Order", fact.Name);
        Assert.Equal(TypeDeclarationKind.Class, fact.Kind);
    }

    [Fact]
    public void Extract_AllFacts_AreMarkedAsSyntaxOrigin()
    {
        // Given
        var source = """
            namespace Shop;

            public interface IOrder { }
            public class Order : IOrder { }
            """;
        var result = CSharpSourceParser.Parse(source, "Order.cs");

        // When
        var facts = TypeDeclarationExtractor.Extract(result.Tree);

        // Then
        Assert.Equal(2, facts.Count);
        Assert.All(facts, f => Assert.Equal(FactOrigin.Syntax, f.Origin));
    }
}
