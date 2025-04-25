# SILQ Language Specification v1

`SILQ` (Simple Interpreted Language for Queries) is a lightweight, expressive language designed to query .NET object data in a simple, human-readable way. It is crafted to provide developers with a minimal, easy-to-learn scripting environment that can be quickly embedded into applications for powerful object querying without the overhead of complex query engines.

---

## Objectives

- **Simplicity First**  
  `SILQ` is built around the idea that querying object data should be easy to write, easy to read, and easy to understand. It avoids unnecessary complexity, focusing on what matters most: expressing conditions and selecting results in a natural, straightforward manner.

- **Natural Syntax**  
  Inspired by familiar concepts from `SQL` and `LINQ`, `SILQ` queries resemble plain English instructions. This lowers the learning curve and makes queries self-explanatory even for developers who have not been extensively trained in query languages.

- **Dynamic Execution**  
  `SILQ` is interpreted at runtime. This makes it highly flexible, ideal for scenarios where queries must be defined dynamically, configured externally, or updated without recompiling an application.

- **Strict and Predictable Behavior**  
  While `SILQ` is dynamically typed, it enforces strict typing on operations like comparisons, arithmetic, and logical conditions to ensure that queries behave consistently and safely.

- **Lightweight Integration**  
  `SILQ` is designed to integrate easily into .NET applications with minimal dependencies. It relies on standard reflection capabilities to access and traverse object properties, making it a perfect fit for internal tooling, user-defined queries, or low-overhead data transformation tasks.

---

## Scope

`SILQ` focuses on a small but powerful set of features:
- Filtering collections based on flexible conditions
- Selecting and projecting fields or computed expressions
- Counting, picking the first, or picking the last matching item
- Accessing nested properties through simple dot notation
- Performing basic logical, arithmetic, and string operations
- Supporting intuitive membership and comparison checks

By intentionally limiting its scope, `SILQ` ensures developers have everything they need to query and shape their data—without overwhelming them with options or complexity.

---

SILQ’s vision is to **empower developers** to work with data inside their applications as naturally and effortlessly as possible, while remaining lightweight, transparent, and easy to extend in future versions.