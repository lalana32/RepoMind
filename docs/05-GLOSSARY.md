# RepoMind — Glossary

This glossary is intended as a practical reference. Definitions will expand as the project develops.

## Compiler and source analysis

**Compiler**  
A toolchain that processes source code and translates it into another representation, while checking language rules and reporting errors.

**Roslyn**  
The .NET compiler platform for C#. It exposes APIs for parsing, compiler diagnostics, symbols, semantic analysis, and compilation.

**Syntax**  
The grammatical structure of source code: declarations, expressions, statements, tokens, and their nesting.

**Syntax Tree**  
A tree-shaped representation of parsed source code. It tells us what constructs appear and how they are nested. It does not, by itself, resolve every name to its actual declaration.

**SyntaxNode**  
A node in a syntax tree representing a language construct, such as a class declaration, method declaration, or invocation expression.

**SyntaxToken**  
A lexical token such as an identifier, keyword, punctuation mark, or operator.

**AST (Abstract Syntax Tree)**  
A general term for a tree representation of program structure. Roslyn exposes a detailed C# syntax tree, including concrete syntax details.

**Semantic Model**  
A Roslyn API that answers semantic questions about syntax in the context of a compilation, such as which symbol a name refers to and what type an expression has.

**Symbol**  
Roslyn's representation of a declared or referenced program element, such as a class, method, property, or namespace. Symbols help distinguish declarations that share the same text name.

**Compilation**  
A Roslyn representation of a C# compilation, including syntax trees, references, options, symbols, and diagnostics.

**Metadata reference**  
A reference to a compiled assembly that the compiler needs in order to understand types and symbols used by source code.

**Static analysis**  
Analysis of source code or compiled representations without running the application being analyzed.

**Method invocation**  
A call expression in source code. Roslyn can often resolve it to a method symbol when the compilation has sufficient information.

## Architecture and graphs

**Dependency**  
A relationship in which one program element relies on or references another. The exact meaning must be defined: type reference, method call, inheritance, project reference, and so on are different relationship types.

**Dependency Graph**  
A graph whose nodes represent entities (such as projects, types, or methods) and whose edges represent defined relationships.

**Call Graph**  
A graph representing possible method-call relationships. A complete runtime call graph is difficult to infer statically in languages with dynamic dispatch, reflection, and runtime behavior.

**Change Impact Analysis**  
An analysis that identifies elements that may be affected by a change to a selected element. Results are potential impact, not a guarantee of runtime consequences.

**Source Location**  
Information identifying where a finding came from, typically a file path and source span or line/column range.

## AI concepts

**AI (Artificial Intelligence)**  
A broad field covering systems that perform tasks associated with intelligence, including language understanding and generation.

**LLM (Large Language Model)**  
A machine-learning model trained on large amounts of text and other data to process and generate language. ChatGPT and Claude are examples of products powered by LLMs.

**Prompt**  
Instructions and context supplied to an AI model.

**Hallucination**  
An AI-generated statement that sounds plausible but is unsupported or incorrect.

**Grounding**  
Providing an AI model with relevant, verifiable information so its answer is based on evidence rather than unsupported generation.

**Embedding**  
A numerical representation of content used to compare semantic similarity. Embeddings may be useful for search, but they are not required for the initial RepoMind analyzer.

## Engineering workflow

**MVP (Minimum Viable Product)**  
The smallest product version that can deliver and validate a meaningful user outcome.

**Proof of Concept (PoC)**  
A small experiment intended to test whether a technical idea is feasible. It is not necessarily production-ready.

**CI (Continuous Integration)**  
Automated checks, commonly builds and tests, run when code changes.

**API (Application Programming Interface)**  
A defined interface through which software components communicate.
