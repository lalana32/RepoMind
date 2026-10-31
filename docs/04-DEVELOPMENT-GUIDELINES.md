# RepoMind — Development Guidelines

## 1. Developer learning goals
The product owner knows C#, .NET, React, and TypeScript, but is new to Roslyn and compiler APIs.

When introducing unfamiliar concepts:
- explain the concept in practical terms
- show a small example
- explain why RepoMind needs it
- distinguish what is known from what is inferred
- avoid dumping compiler theory that is not needed for the current task

The goal is for the developer to understand and be able to maintain the implementation.

## 2. AI-assisted development workflow
For each task:
1. State the task and acceptance criteria.
2. Inspect relevant existing code and documentation.
3. Propose a small implementation plan.
4. Explain important design choices and alternatives.
5. Implement only the agreed scope.
6. Run relevant tests and report actual results.
7. Review edge cases and limitations.
8. Update documentation if behavior or decisions changed.

Do not ask the AI agent to generate large features without intermediate review.

## 3. Code quality
- Use clear names and cohesive responsibilities.
- Avoid unnecessary abstractions and speculative extensibility.
- Keep compiler analysis separate from presentation and persistence.
- Prefer explicit result types for diagnostics and unresolved references.
- Avoid swallowing exceptions or hiding analysis failures.
- Handle cancellation and resource limits where relevant.
- Keep public APIs and serialized models deliberate and versionable.

## 4. Testing
Tests should cover:
- normal cases
- empty and malformed input
- multiple files
- duplicate names in different namespaces
- overloaded methods
- inheritance and interface implementation
- unresolved symbols and missing references
- generic types and nested types as supported
- analyzer diagnostics and failure paths

Do not claim full correctness based only on happy-path tests.

## 5. Static-analysis honesty
Always distinguish:
- syntax extracted directly from source
- symbols resolved by Roslyn
- heuristic patterns
- unresolved or unsupported cases

Never claim that a static analyzer can always reconstruct runtime behavior. Reflection, dynamic dispatch, dependency injection, generated code, conditional compilation, and runtime configuration can make this impossible or incomplete.

## 6. Security
- Treat analyzed repositories as untrusted input.
- Do not execute analyzed code.
- Avoid running arbitrary build targets or repository scripts.
- Validate and constrain file paths.
- Consider symlinks, oversized files, deeply nested structures, and resource exhaustion.
- Do not upload private source code to an external LLM without explicit user consent and a clear data-handling design.
- Keep secrets out of source control and logs.

## 7. Git and documentation
- Use small, meaningful commits.
- Do not commit secrets, generated build output, or local IDE state.
- Update README and relevant docs when setup or behavior changes.
- Record accepted architectural decisions in `06-DECISION-LOG.md`.
- Keep the roadmap current; mark work complete only when exit criteria are met.

## 8. Definition of done
A task is done when:
- acceptance criteria are met
- code builds
- relevant tests pass
- failure/unsupported cases are considered
- behavior is documented where appropriate
- the developer can explain the important implementation choices
