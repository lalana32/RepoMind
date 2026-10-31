# RepoMind — Product Requirements (Initial)

## 1. Purpose
This document defines the initial product scope and expected user-facing behavior. Requirements are provisional and should be revised as we validate the product.

## 2. Primary user workflow
1. The user selects a local .NET solution or another explicitly supported repository source.
2. RepoMind discovers supported solution and project files.
3. The analyzer processes source files without executing the analyzed application.
4. The user sees analysis status and any errors or limitations.
5. The user explores projects, namespaces, types, methods, and relationships.
6. The user selects a symbol and explores incoming and outgoing dependencies.
7. The user requests a change-impact view for that symbol.
8. Later, the user may ask natural-language questions answered using analysis evidence.

The first proof of concept will not implement this entire workflow. It will validate the core analysis capability first.

## 3. Functional requirements — staged

### P0: Roslyn proof of concept
- Parse C# source text using Roslyn.
- Identify class and interface declarations.
- Identify method and constructor declarations.
- Extract declared base types and implemented interfaces syntactically.
- Introduce semantic analysis to resolve symbols in a small multi-file compilation.
- Include tests for representative examples and edge cases.

### P1: Solution analysis engine
- Load supported `.sln` / project structures using compiler workspace APIs where appropriate.
- Analyze multiple source files and projects.
- Extract symbol identities and source locations.
- Resolve type references and method invocations where semantic information is available.
- Record analysis diagnostics and unresolved references.
- Produce a versioned, serializable analysis result.

### P2: Dependency exploration
- Represent projects, namespaces, types, and methods as entities.
- Represent supported relationships as typed edges.
- Support incoming and outgoing dependency queries.
- Preserve source locations and evidence for relationships.
- Avoid presenting uncertain relationships as confirmed facts.

### P3: Web application
- Show repositories and analysis runs.
- Show project and symbol browsers.
- Visualize dependency relationships.
- Allow selection, filtering, search, and navigation to source locations where available.
- Show analysis status, diagnostics, and coverage limitations.

### P4: Change impact analysis
- Starting from a selected symbol, find supported direct and transitive dependents.
- Allow bounded traversal and filtering by relationship type.
- Explain why each item appears in the impact set.
- Label results as potential impact, not a guarantee that a runtime behavior will change.

### P5: LLM-assisted explanation
- Answer questions using structured analyzer output as grounding context.
- Cite the relevant symbols, relationships, and source locations in the answer.
- Clearly say when the analysis does not contain enough evidence.
- Do not let the LLM create or modify the underlying dependency graph.

## 4. Non-functional requirements
- **Correctness:** deterministic findings should be testable.
- **Traceability:** findings should link to source file and source span where possible.
- **Security:** never execute analyzed repository code as part of static analysis.
- **Resilience:** malformed files and unsupported project configurations should produce useful diagnostics rather than unexplained crashes.
- **Performance:** measure analysis duration and memory on representative repositories before setting targets.
- **Maintainability:** keep analysis logic separate from web/API concerns.
- **Transparency:** expose analyzer limitations and unresolved symbols.

## 5. Explicitly out of scope for the first MVP
- Support for multiple programming languages.
- Runtime tracing or production telemetry.
- Automatic code modification or refactoring.
- Complete reflection or dynamic-dispatch resolution.
- Guaranteed full call graph reconstruction.
- Complex team accounts, billing, or enterprise permissions.
- AI-generated architecture claims without analyzer evidence.

## 6. Acceptance philosophy
A feature is not complete merely because it works on one hand-written example. Each analysis capability should have:
- a written definition of what it supports
- positive tests
- negative or unsupported-case tests
- traceable output
- documented limitations
