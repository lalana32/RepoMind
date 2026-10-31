# Roslyn — osnove sintakse (RM-002)

## Node, token i trivia

**Node (`SyntaxNode`)** predstavlja čvor, tj. smislenu cjelinu jezika (deklaraciju, izraz, naredbu) koja se sastoji od drugih node-ova i tokena. Primjeri: `ClassDeclaration`, `MethodDeclaration`, `ParameterList`, `Block`. Node uvijek ima djecu.

**Token (`SyntaxToken`)** je najmanja jedinica u sintaksi i nema djecu (list stabla). Primjeri: ključna riječ (`public`, `class`), ime (`OrderService`), zagrada (`{`), operator (`:`, `=`). Tekst tokena se čita sa `token.Text`.

**Trivia (`SyntaxTrivia`)** je sve između tokena što ne mijenja značenje koda: razmaci, novi redovi, komentari (`//`, `/* */`), XML doc komentari (`///`) i direktive (`#if`, `#region`). Trivia nije samostalna, nego je zakačena za token:

- **leading trivia**: ispred tokena (npr. komentar iznad klase je leading trivia prvog tokena klase, `public`)
- **trailing trivia**: iza tokena (npr. razmak poslije `class`, novi red poslije `}`)

Roslyn čuva triviju da bi stablo moglo tačno da rekonstruiše originalni tekst. RepoMind je uglavnom ignoriše.

### Primjer

`public class OrderService : BaseService`

**Tokeni (5):** `public`, `class`, `OrderService`, `:`, `BaseService`

**Node-ovi (4, ugniježđeni):**
- `ClassDeclaration`: cijela linija, "ovdje se deklariše klasa"
- `BaseList`: `: BaseService`, "lista baznih tipova"
- `SimpleBaseType`: `BaseService`, "jedan unos u base listi"
- `IdentifierName`: `BaseService`, "ovdje se koristi neko ime"

**Trivia:** razmaci između tokena, npr. razmak poslije `class` je trailing trivia tokena `class`.

```
ClassDeclaration            (node)
├─ public                   (token)
├─ class                    (token)
├─ OrderService             (token)  ← ime se deklariše, pa je samo token
└─ BaseList                 (node)
   ├─ :                     (token)
   └─ SimpleBaseType        (node)
      └─ IdentifierName     (node)   ← ime se koristi, pa je umotano u node
         └─ BaseService     (token)
```

`OrderService` je samo token jer se tu ime **deklariše**. `BaseService` je umotan u `IdentifierName` jer se tu ime **koristi**, a nad takvim node-om se kasnije pita semantika "na šta se ovo odnosi?".

## Node tipovi koje koristimo (referenca, ne učiti napamet)

Obrazac: deklaracija nečega je `<Nešto>Declaration`. sharplab prikazuje ime bez sufiksa, a tip u C# kodu ima sufiks `Syntax`. Kad nisi siguran, nalijepi kod u sharplab.io (Syntax Tree) i pročitaj.

| Konstrukcija                      | sharplab prikaz                  | Tip u C# kodu                          |
| --------------------------------- | -------------------------------- | -------------------------------------- |
| Cijeli fajl (korijen stabla)      | `CompilationUnit`                | `CompilationUnitSyntax`                |
| `namespace Shop { ... }`          | `NamespaceDeclaration`           | `NamespaceDeclarationSyntax`           |
| `namespace Shop;` (file-scoped)   | `FileScopedNamespaceDeclaration` | `FileScopedNamespaceDeclarationSyntax` |
| Klasa                             | `ClassDeclaration`               | `ClassDeclarationSyntax`               |
| Interfejs                         | `InterfaceDeclaration`           | `InterfaceDeclarationSyntax`           |
| Metoda                            | `MethodDeclaration`              | `MethodDeclarationSyntax`              |
| Konstruktor                       | `ConstructorDeclaration`         | `ConstructorDeclarationSyntax`         |
| `: BaseService, IOrderService`    | `BaseList`                       | `BaseListSyntax`                       |
| Jedan unos u base listi           | `SimpleBaseType`                 | `SimpleBaseTypeSyntax`                 |
| Upotreba imena (npr. tip `Order`) | `IdentifierName`                 | `IdentifierNameSyntax`                 |

Napomena: oba oblika namespace-a moraju biti podržana pri ekstrakciji (RM-005).

## Šta syntax tree NE zna

Syntax tree zna **šta piše i gdje**, ali ne zna **šta to znači**. Vidi samo tekst jednog fajla i njegovu strukturu. Ne razrješava imena do njihovih deklaracija.

Analogija: u rečenici "Petar je kupio auto" gramatika kaže da je "Petar" subjekat, ali ne kaže **koji** Petar. Za to treba kontekst van rečenice.

```csharp
public class OrderService : BaseService, IOrderService
{
    public void Place(Order order) { }
}
```

| Pitanje | Stablo zna? | Zašto |
|---|---|---|
| Postoji klasa `OrderService` sa base listom od dvije stavke? | Da | Piše u tekstu. |
| Da li je `BaseService` klasa ili interfejs? | Ne | Obje stavke imaju isti oblik (`SimpleBaseType` → `IdentifierName`). Odgovor je u fajlu gdje je `BaseService` deklarisan. Prefiks "I" je samo konvencija, tj. nagađanje. |
| Gdje je deklarisan `Order`, i koji `Order` (`Shop.Order` ili `Billing.Order`)? | Ne | Stablo vidi samo ime "Order". |
| Da li `Order` uopšte postoji? | Ne | Stablo bi izgledalo isto i da `Order` ne postoji nigdje. |

Na ova pitanja odgovara tek **kompilacija** (svi fajlovi + reference) i `SemanticModel` (RM-008 do RM-013).

### Sintaksna vs semantička greška

| Greška | Vrsta | Značenje |
|---|---|---|
| `CS1513: } expected` | **Sintaksna** (parsiranje) | Tekst nije ispravna C# gramatika. Roslyn i dalje napravi stablo, ali ubaci "nedostajući" (missing) token i prijavi grešku. |
| `CS0246: The type or namespace name 'Order' could not be found` | **Semantička** (kompilacija) | Tekst je ispravan i stablo je kompletno, ali kompajler ne može da nađe na šta se ime `Order` odnosi. |

Dokaz: u sharplab-u je uz CS0246 stablo bilo potpuno ispravno. sharplab osim parsiranja i kompajlira kod, pa su CS0246 greške došle iz kompilacije, ne iz stabla.

### Pravilo za RepoMind

Kada RepoMind kaže "`OrderService` implementira `IOrderService`", ta tvrdnja mora doći iz semantike, ne iz sintakse. Iz same sintakse to bi bilo nagađanje. Ime koje se ne može razriješiti prijavljuje se kao **Unresolved** (RM-013), a ne pogađa se.

## Zašto RepoMind-u treba syntax tree

- daje pouzdanu strukturu (komentar ili string koji sadrži "class" nije klasa, za razliku od regex-a)
- daje deklaracije: klase, interfejse, metode, konstruktore → symbol browser
- daje lokaciju svakog node-a (fajl, linija) → dokaz (evidence) za svaku vezu
- daje mjesta gdje se imena koriste → nad njima `SemanticModel` kasnije odgovara "na šta se ovo odnosi?"
