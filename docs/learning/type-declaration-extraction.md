# Ekstrakcija deklaracija tipova (RM-005)

Kod: `src/RepoMind.Poc.Analysis/TypeDeclarationExtractor.cs`

## Šta se izvlači

Za svaku klasu i interfejs u jednom syntax tree-u izvlači se jedan `TypeDeclarationFact`:

| Polje | Izvor | Napomena |
|---|---|---|
| `Name` | `Identifier.Text` | Samo ime, bez namespace-a. |
| `Kind` | tip node-a | `Class` ili `Interface`. |
| `Namespace` | svi `BaseNamespaceDeclarationSyntax` preci | Ugniježdeni blokovi se spajaju: `Shop.Orders`. Pokriva i block-scoped i file-scoped oblik. `null` ako tip nije u namespace-u. |
| `ContainingType` | svi `BaseTypeDeclarationSyntax` preci | Za ugniježdene tipove, npr. `Order.Line`. `null` za tip na vrhu. |
| `Modifiers` | `Modifiers` tokeni | Redoslijed kao u izvornom kodu, npr. `["public", "partial"]`. |
| `Line`, `Column` | `Identifier.GetLocation().GetLineSpan()` | Roslyn broji od 0, RepoMind prijavljuje od 1 (kao editor). Pokazuje na ime tipa, ne na atribute ili modifikatore. |
| `Origin` | uvijek `FactOrigin.Syntax` | Činjenica dolazi iz teksta jednog fajla, ne iz kompilacije. |

## Šta se namjerno NE izvlači

`record`, `record struct`, `struct` i `enum` su van opsega PoC-a i ignorišu se.

Zamka: `RecordDeclarationSyntax` i `StructDeclarationSyntax` takođe nasljeđuju `TypeDeclarationSyntax`. Zato extractor eksplicitno provjerava `ClassDeclarationSyntax` i `InterfaceDeclarationSyntax`, umjesto da prihvati sve što je `TypeDeclarationSyntax`. Negativni test to čuva.

## Partial klase: zašto dva fakta, a ne jedan

```csharp
public partial class Order { }   // Order.cs
partial class Order { }          // Order.Generated.cs
```

Extractor vraća **dva** fakta, po jedan za svaki dio. Iz sintakse se ne može dokazati da su to dijelovi istog tipa: to važi samo ako su u istom namespace-u, istom assembly-ju i imaju isti broj generičkih parametara, a to zna tek kompilacija. Spajanje dijelova u jedan tip radi se preko `SemanticModel` (jedan `INamedTypeSymbol` ima više `DeclaringSyntaxReferences`).

## Ograničenja

- `Column` broji znakove, pa se tab računa kao jedna kolona (editor ga može prikazati kao 4).
- `Namespace` i `ContainingType` su tekst iz izvornog koda, ne razriješeni simboli. `using` alias-i i `global using` se ne uzimaju u obzir.
- Generički parametri se ne prijavljuju: `Repository<T>` daje ime `Repository`.
