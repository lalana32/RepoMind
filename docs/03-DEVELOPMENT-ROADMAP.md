# RepoMind — Development Roadmap

## Working capacity
Planning assumption: approximately 3–5 focused hours per day, around 5–6 days per week. The roadmap is an estimate, not a deadline promise. Extend a phase when correctness or understanding requires it.

## Phase 0 — Project setup
- Create Git repository and documentation structure.
- Verify installed .NET SDK and development environment.
- Establish basic Git workflow.
- Read project instructions and confirm scope.

**Exit criteria:** repository is initialized; documentation is present; environment is understood.

## Phase 1 — Roslyn learning proof of concept (estimated 2–4 days)
- Create a small .NET console application.
- Parse C# source using Roslyn.
- Extract class, interface, method, constructor, and base-type syntax.
- Inspect syntax trees in a debugger.
- Create a small multi-file compilation.
- Use SemanticModel to resolve symbols and references.
- Add xUnit tests.

**Exit criteria:** the developer can explain syntax parsing versus semantic analysis; tests demonstrate extraction and symbol resolution.

## Phase 2 — Analysis engine foundation (estimated 1–2 weeks)
- Investigate solution/project loading options.
- Load representative .NET projects.
- Handle multiple source files and project references.
- Extract stable symbol identifiers and source locations.
- Resolve selected type references and method invocations.
- Record diagnostics and unresolved references.
- Define and test a versioned analysis result model.

**Exit criteria:** analyzer produces a structured, traceable result for at least one real repository.

## Phase 3 — Backend and persistence (estimated 1 week)
- Create ASP.NET Core API.
- Define analysis lifecycle and status model.
- Persist repository and analysis metadata/results.
- Expose endpoints for browsing projects, symbols, relationships, and diagnostics.
- Add integration tests.

**Exit criteria:** a client can request an analysis and retrieve its results through the API.

## Phase 4 — Frontend and graph exploration (estimated 1–2 weeks)
- Create React + TypeScript application.
- Build repository/analysis overview.
- Build project and symbol browser.
- Add graph visualization.
- Add selection details, search, and filters.
- Display source evidence and unresolved diagnostics.

**Exit criteria:** a user can explore a real analysis without reading raw JSON.

## Phase 5 — Change impact analysis (estimated 1 week)
- Define supported traversal semantics.
- Calculate direct and transitive dependents.
- Add depth and relationship filters.
- Explain why each symbol is included.
- Test cycles, large graphs, and ambiguous cases.

**Exit criteria:** impact view is reproducible, explainable, and clearly qualified as potential impact.

## Phase 6 — LLM-assisted explanations (estimated 3–5 days, after core)
- Learn basic LLM/API concepts.
- Select a provider and define data-handling constraints.
- Retrieve relevant graph facts and source snippets.
- Generate evidence-grounded explanations.
- Show references to underlying findings.
- Test unsupported questions and hallucination-prone cases.

**Exit criteria:** the LLM explains known findings and admits when evidence is insufficient.

## Phase 7 — Hardening and demonstration (estimated 1–2 weeks)
- Test on at least one repository not used during implementation.
- Measure runtime and memory.
- Improve failure handling and security boundaries.
- Add documentation and reproducible setup.
- Deploy a demo environment if appropriate.
- Prepare a short, technically honest product demonstration.

**Exit criteria:** a new user can run or access the demo, analyze a supported repository, and understand the product's limitations.

## Priority rule
If time becomes constrained, prioritize:
1. correctness of analysis
2. traceability and explanation of findings
3. useful exploration workflow
4. reliability and security
5. visual polish
6. LLM features

Do not sacrifice analyzer correctness to add AI features or decorative UI.
