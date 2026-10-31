# RepoMind — Project Overview

## 1. Product vision
RepoMind is a codebase intelligence platform that helps developers understand the structure and relationships inside real software repositories.

It should turn source code into navigable, evidence-backed information about:
- projects and namespaces
- classes, interfaces, methods, and other relevant symbols
- references and dependencies between symbols
- architectural structure
- potential impact of changing a symbol

The product should help a developer answer questions such as:
- Where is this class used?
- Which methods call this method?
- Which parts of the solution depend on this project?
- What could be affected if I change this interface or service?
- How is an API endpoint connected to application services and data access?

## 2. Problem
In medium and large codebases, understanding dependencies and change impact can require significant manual exploration. Existing IDE navigation is useful, but RepoMind aims to provide a higher-level, visual and cross-project view that makes relationships easier to explore.

This problem statement is a product hypothesis. It must be validated against real developers and real repositories rather than treated as proven market demand.

## 3. Initial target user
The initial target user is a software developer or technical lead working with C#/.NET codebases, especially solutions containing multiple projects and layers.

Potential later users:
- engineering managers
- architects
- teams onboarding developers to unfamiliar systems
- consultants maintaining legacy .NET applications

## 4. Product principles
1. **Evidence before explanation:** show relationships supported by source or compiler analysis.
2. **Useful over flashy:** visualizations must answer real engineering questions.
3. **Honest uncertainty:** identify unresolved, dynamic, or heuristic relationships.
4. **Developer control:** users can inspect the source locations behind findings.
5. **Incremental analysis:** start with a narrow, reliable language and feature set.
6. **Security by design:** analyzed repositories are untrusted input.

## 5. Initial product scope
The first version focuses on C#/.NET static analysis. It is not intended to be a universal multi-language platform at the beginning.

The initial product direction includes:
- analyzing a real .NET solution
- extracting symbols and relationships
- visualizing project and symbol dependencies
- exploring a selected class or method
- showing potential change impact
- later, using an LLM to explain findings grounded in analyzer output

## 6. What RepoMind is not
At the beginning, RepoMind is not:
- an AI chatbot that guesses how code works
- a replacement for an IDE or debugger
- a runtime observability platform
- a vulnerability scanner
- a complete architecture recovery tool for every programming language
- a tool that guarantees every possible dependency can be found

## 7. Success criteria
The project should be considered successful as an engineering demonstration when it can:
- analyze at least one real, non-trivial .NET repository
- produce correct, traceable symbol and dependency information for its supported cases
- clearly communicate unsupported or unresolved cases
- provide an interactive way to explore the resulting structure
- demonstrate a useful change-impact workflow
- include automated tests, documentation, and a reproducible demo

Performance, correctness, and usefulness must be measured on actual repositories. Do not claim broad scalability or complete accuracy without evidence.

## 8. Product validation
During development, seek feedback from developers and technical leads. Ask them to try concrete workflows, not just react to screenshots:
- locate usages of a selected symbol
- understand a service's dependencies
- trace a selected endpoint into application layers
- inspect the possible impact of a proposed change

Record feedback and revise priorities accordingly.
