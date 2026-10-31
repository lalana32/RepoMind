# RepoMind

Codebase intelligence for C#/.NET repositories, built on deterministic Roslyn analysis. Early stage: no application code yet. Start with [CLAUDE.md](CLAUDE.md) and the documents in [docs/](docs/).

## Development environment

Recorded 2026-10-31 (`dotnet --info`):

| Item | Version / choice |
|---|---|
| .NET SDK (default) | 10.0.403 (LTS); 7.0, 8.0 and 9.0 SDKs also installed |
| Runtime | Microsoft.NETCore.App 10.0.14 |
| Target framework | `net10.0` (current LTS) |
| Editor | VS Code with C# Dev Kit; Visual Studio Community 2022 17.5.4 is installed but too old for `net10.0` projects |
| Syntax-tree viewer | [sharplab.io](https://sharplab.io) (select "Syntax Tree" output); optional: Syntax Visualizer from the ".NET Compiler Platform SDK" Visual Studio component, which requires a Visual Studio version that supports .NET 10 |
| OS | Windows 10 (x64) |

No `global.json` is committed, so the newest installed SDK is used.

## Git workflow

`main` should always build and pass its tests. Work on one Trello card at a time, on a short-lived branch named after the card (for example `rm-005-extract-types`), and merge it back into `main` once the card's acceptance criteria are met. Make small commits with an imperative subject line that starts with a conventional prefix and the card ID, for example `feat(RM-005): extract class and interface declarations`. Prefixes: `feat`, `fix`, `test`, `docs`, `chore`, `refactor`. Never commit secrets, build output (`bin/`, `obj/`) or local IDE state. `.gitignore` covers these.
