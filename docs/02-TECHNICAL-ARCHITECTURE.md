# RepoMind — Technical Architecture (Proposed)

## 1. Status
This is a proposed direction, not a frozen architecture. The first milestone is a standalone Roslyn proof of concept. Validate technical assumptions before implementing the complete system.

## 2. Proposed technology choices

| Area | Initial choice | Notes |
|---|---|---|
| Language analysis | C# / Roslyn | Primary differentiating engine |
| Backend | ASP.NET Core | Familiar application/API layer |
| Frontend | React + TypeScript | Interactive product UI |
| Graph visualization | React Flow (`@xyflow/react`) | Candidate for node-and-edge exploration |
| Persistence | PostgreSQL | Store projects, analysis runs, and normalized or serialized results as justified |
| Tests | xUnit | .NET unit and integration tests |
| Packaging | Docker | Introduce when needed for reproducible deployment |
| AI explanations | LLM API, later | Grounded in analyzer output; provider decision deferred |

Do not treat every choice as mandatory. Revisit choices when evidence or requirements justify it.

## 3. High-level logical components

### A. Analysis Engine
Responsible for:
- discovering supported solution/project inputs
- creating Roslyn compilations/workspaces
- extracting symbols and relationships
- recording source locations and diagnostics
- producing a structured analysis result

This component should be independent of HTTP controllers and frontend concerns.

### B. Application/API layer
Responsible for:
- accepting analysis requests
- managing analysis lifecycle
- exposing results and queries
- validating inputs and enforcing safety boundaries

### C. Persistence
Responsible for storing:
- repository/project metadata
- analysis run metadata
- analyzer version and supported capability version
- structured findings and diagnostics

Choose relational tables, JSON storage, or a hybrid based on actual query patterns. Do not prematurely design a complex graph database.

### D. Frontend
Responsible for:
- repository and analysis navigation
- project/symbol browsing
- dependency visualization
- source evidence and diagnostics
- change-impact exploration

### E. Intelligence layer (later)
Responsible for:
- retrieving relevant structured findings
- preparing bounded context for an LLM
- generating explanations grounded in retrieved evidence
- preserving references to evidence in responses

The LLM is not the source of truth for code relationships.

## 4. Analysis concepts
The analyzer should distinguish:
- **Syntax facts:** what is explicitly written in source syntax.
- **Semantic facts:** what Roslyn can resolve to a symbol/type in a compilation.
- **Heuristic findings:** patterns inferred by rules that may not be conclusive.
- **Unresolved findings:** references that could not be resolved due to missing dependencies, unsupported project configuration, conditional compilation, or other limitations.

Do not merge these categories into one unqualified dependency type.

## 5. Important technical constraints
- A syntax tree describes parsed source structure; it does not by itself prove symbol identity or runtime behavior.
- Semantic analysis depends on a sufficiently configured compilation and its references.
- Dynamic dispatch, reflection, runtime dependency injection, generated code, conditional compilation, and dynamic loading can limit static conclusions.
- A method invocation syntax can often be resolved to a method symbol, but a complete runtime call graph is a harder problem.
- Loading a solution for analysis must not build or execute arbitrary targets from an untrusted repository without a deliberate security review.
- Repository input should be treated as untrusted. Define path restrictions, file-size limits, symlink handling, and resource limits before accepting arbitrary uploads.

## 6. Proposed data model direction
Do not finalize the schema before the analyzer output is understood. Likely concepts include:
- Repository
- AnalysisRun
- Project
- SourceFile
- Symbol (with stable identity within an analysis)
- Relationship (typed edge)
- Diagnostic
- SourceLocation

Each relationship should record:
- relationship type
- source symbol
- target symbol, if resolved
- evidence/source location
- confidence or resolution category where relevant
- analyzer capability/version

## 7. Initial dependency relationship types
Potential initial relationship types:
- project references project
- class inherits class
- class implements interface
- type references type
- method invokes method (when semantically resolved)
- member belongs to type

Define precise semantics for every edge. Do not label all references as generic "depends on" without preserving their meaning.

## 8. Deployment and scaling
Start with a modular monolith and a separately executable analyzer component if needed. Do not introduce microservices or distributed infrastructure until measured workloads justify them.

Analysis may be long-running. A background job mechanism may eventually be needed, but choose the simplest reliable approach for the MVP.
