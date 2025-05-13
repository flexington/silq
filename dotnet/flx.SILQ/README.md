# SILQ

**SILQ** (Simple Interpreted Language for Queries) is a lightweight, expressive language designed to query object data in a simple, human-readable way. It provides developers with a minimal, easy-to-learn scripting environment that can be quickly embedded into applications for powerful object querying—without the overhead of complex query engines.

## What is SILQ for?

SILQ is ideal for:
- Filtering and transforming in-memory objects using a natural, SQL/English-like syntax
- Embedding dynamic, user-defined queries in your applications
- Internal tooling, configuration, or low-overhead data transformation tasks

SILQ is inspired by SQL and LINQ, but is intentionally minimal and runtime-interpreted. It is designed to be simple, predictable, and easy to integrate.

## Key Features
- Filter collections with flexible conditions
- Select and project fields or computed expressions
- Count, pick the first, or pick the last matching item
- Access nested properties with dot notation
- Perform logical, arithmetic, and string operations
- Use intuitive membership and comparison checks

## Example Usage

Suppose you have a collection of `Person` objects in your .NET application:

```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
}
```

You can use SILQ to query this data:

### 1. Select all people over 18
```silq
from people where Age > 18;
```

### 2. Select names of people in London
```silq
from people where City == "London" select { Name };
```

### 3. Count people under 30
```silq
count from people where Age < 30;
```

### 4. Get the first person named "Alice"
```silq
first from people where Name == "Alice";
```

---

For more details, see the [Specs]([./.docfx/specs/index.md](https://flexington.github.io/silq/specs/index.html)) and [API](https://flexington.github.io/silq/api/flx.SILQ.Core.html) documentation.
