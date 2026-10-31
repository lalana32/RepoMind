# RepoMind — Decision Log

This log records decisions, their status, rationale, and alternatives. Proposed choices are not final until validated.

## D-001 — Start with C#/.NET only
- **Status:** Accepted for initial scope
- **Decision:** The first analyzer targets C#/.NET repositories.
- **Rationale:** The developer already knows .NET; Roslyn offers strong compiler APIs; a narrow scope improves the chance of building a reliable product.
- **Alternatives:** Multi-language support from the beginning.
- **Revisit when:** The C# workflow is useful and validated, and there is a concrete reason to add another language.

## D-002 — Use Roslyn for C# source analysis
- **Status:** Accepted for proof of concept
- **Decision:** Use Roslyn APIs for parsing and semantic analysis rather than regex or LLM-based dependency guessing.
- **Rationale:** Roslyn understands C# syntax and compiler symbols and can provide structured, testable findings.
- **Limitations:** Results depend on compilation context and cannot fully determine all runtime behavior.
- **Revisit when:** A specific analysis need cannot be met by the selected Roslyn APIs.

## D-003 — Keep the analyzer separate from the web API
- **Status:** Proposed; validate during implementation
- **Decision:** Keep analysis logic in a distinct component/module from HTTP and UI code.
- **Rationale:** Analysis has different responsibilities, resource needs, and testing requirements.
- **Alternatives:** Put analyzer logic directly into API services.
- **Revisit when:** The proof of concept reveals a simpler structure that still preserves separation of concerns.

## D-004 — Use React and TypeScript for the frontend
- **Status:** Proposed
- **Decision:** Use the developer's existing frontend stack.
- **Rationale:** Familiarity and suitability for interactive graph exploration.
- **Revisit when:** A concrete UI requirement suggests a better option.

## D-005 — Use React Flow for graph visualization
- **Status:** Proposed
- **Decision:** Evaluate `@xyflow/react` for interactive node-and-edge visualization.
- **Rationale:** It is designed for interactive graph-like interfaces.
- **Revisit when:** Prototype performance, layout, licensing, or interaction needs show it is unsuitable.

## D-006 — Defer LLM integration until after deterministic analysis
- **Status:** Accepted
- **Decision:** Do not make an LLM part of the first analyzer milestone.
- **Rationale:** The core product value depends on trustworthy code facts. AI explanations are more useful when grounded in those facts.
- **Revisit when:** The analyzer produces structured results that can be retrieved and cited.

## D-007 — Start as a modular monolith
- **Status:** Proposed
- **Decision:** Avoid microservices and distributed infrastructure in the MVP.
- **Rationale:** The product is early-stage and does not yet have demonstrated scaling requirements.
- **Alternatives:** Separate deployable services and message brokers from day one.
- **Revisit when:** Measured workloads or operational requirements justify additional infrastructure.

## Decision record template

### D-XXX — Short decision title
- **Status:** Proposed / Accepted / Superseded
- **Decision:**
- **Rationale:**
- **Alternatives considered:**
- **Consequences:**
- **Revisit when:**
