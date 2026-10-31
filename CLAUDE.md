# RepoMind — Claude Code Instructions

## Project context
RepoMind is an early-stage codebase intelligence product. Its initial focus is static analysis of real C#/.NET repositories. The goal is to extract reliable structural facts from source code and present them through useful architecture and dependency exploration.

Read these documents before making significant changes:
- `docs/00-PROJECT-OVERVIEW.md`
- `docs/01-PRODUCT-REQUIREMENTS.md`
- `docs/02-TECHNICAL-ARCHITECTURE.md`
- `docs/03-DEVELOPMENT-ROADMAP.md`
- `docs/04-DEVELOPMENT-GUIDELINES.md`
- `docs/05-GLOSSARY.md`
- `docs/06-DECISION-LOG.md`

## Working rules
1. Work in small, reviewable increments. Do not implement the whole product in one task.
2. Before substantial implementation, summarize the intended change, files affected, key design choices, alternatives, and risks.
3. Do not silently make major architectural decisions. Propose them and explain trade-offs first.
4. Prefer understandable, maintainable code over clever abstractions or premature generalization.
5. Add or update automated tests for behavior changes. Do not claim tests pass unless you ran them and observed the result.
6. Do not invent analysis results. Distinguish facts proven by Roslyn from incomplete or heuristic findings.
7. Do not use an LLM to infer source-code dependencies when deterministic compiler information is available.
8. Never execute code from a repository being analyzed. Treat analyzed repositories as untrusted input.
9. Do not add infrastructure (microservices, message brokers, Redis, Kubernetes, etc.) unless a demonstrated requirement justifies it.
10. Keep documentation aligned with the implementation. Update the decision log when an accepted architectural decision changes.
11. Do not add features outside the current task without asking first.
12. Explain unfamiliar compiler/Roslyn concepts in practical terms. The product owner knows .NET and React but is new to compiler APIs and static-analysis tooling.

## Current development stage
The project has not yet established its implementation architecture in code. The first milestone is a small Roslyn proof of concept. Do not assume that the full planned stack or production architecture has already been implemented.

## First task
Build a small, tested C# console proof of concept that parses C# source and extracts:
- class declarations
- interface declarations
- method declarations
- constructors
- base types and implemented interfaces

Then introduce `SemanticModel` to resolve symbols across a small multi-file example. Keep this as a learning-focused proof of concept; do not build the web app, database, repository upload flow, or LLM integration yet.

## Communication style
Be direct and technically honest. State uncertainty and limitations. Do not overstate what static analysis can prove. When explaining code, explain the important concepts without overwhelming the developer with compiler theory.
