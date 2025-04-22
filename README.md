# SILQ Language Specification (v1)

## Overview

**SILQ** stands for **Simple Interpreted Language for Queries**.
It is a lightweight, dynamically evaluated scripting language for querying object graphs in .NET applications.

The design emphasizes:

- Readability and simplicity
- Natural query syntax
- Strictly typed operators
- Minimal keywords
- Easy reflection-based access to .NET objects

## Syntax Summary

| Keyword | Purpose                            |
| ------- | ---------------------------------- |
| from    | Defines the source collection      |
| where   | Filters items using expressions    |
| select  | Projects output fields/expressions |
| first   | Returns the first matching item    |
| last    | Returns the last matching item     |
| count   | Returns the count of matches       |

## BNF Syntax Definition

Custom BNF notation:

- Rules: `name → symbol`
- Terminals: `"quoted"`
- Non-Terminals: `lowercase`
- Grouping: `(...)`
- Repitition: `*`

```bnf
program             → query* EOF ;

query               → modifier "from" identifier where select projection alias? ";";

modifier            → "first" | "last" | "count";

where               → "where" ( expression | function) ;

select              → "select" projection | ;

projection          → member | "{" member* "}" ;

function            → "contains" | "in" | "startsWith" | "endsWith";

alias               → "as" identifier;

expression          → equiality ;

equality            → comparison ( ( "!=" | "==" ) comparison )* ;

comparison          → term ( ( ">" | ">=" | "<" | "<=" ) term)* ;

term                → factor ( ( "-" | "+" ) factor)* ;

factor              → unary ( ( "/" | "*" ) unary)* ;

unary               → ( "!" | "-" ) unary | primary ;

primary             → literal | "(" expression ")" ;

member              → object "." member ( "." member)* ;

literal             → number | string | bool | "null" | array ;

bool                → true | false ;

number              → 42 | 3.14 | -7 ;

string              → "Hello" | "World" | "foo" ;

array               → [ literal ] ;
```

## Identifiers

- Must match: `^[A-Za-z_][A-Za-z0-9_]*$`
- ASCII only
- Case-Sensitive
- Cannot be reserver words
- User for Variables, Properties, Aliases

## Operators

### Comparison (strictly typed)

| Operator | Meaning          |
| -------- | ---------------- |
| ==       | Equal            |
| !=       | Not equal        |
| >        | Greater than     |
| <        | Less than        |
| >=       | Greater or equal |
| <=       | Less or equal    |

### Logical (boolean only, short-circuiting)

| Operator | Meaning |
| -------- | ------- |
| and      | AND     |
| or       | OR      |
| not      | NOT     |

### Arithmetic (numeric only)

| Operator | Meaning        |
| -------- | -------------- |
| +        | Addition       |
| -        | Subtraction    |
| \*       | Multiplication |
| /        | Division       |
| %        | Modulo         |

### Collection / String

| Keyword    | Meaning                       |
| ---------- | ----------------------------- |
| in         | Membership check              |
| contains   | Substring/element check       |
| startsWith | String prefix                 |
| endsWith   | String suffix                 |
| matches    | Regular Expression comparison |

| Expression  | Meaning     |
| ----------- | ----------- |
| is null     | Equals null |
| is not null | Not null    |

### Literals

| Type    | Example                   |
| ------- | ------------------------- |
| Number  | 42, 3.14                  |
| String  | "hello", supports escapes |
| Boolean | true, false               |
| Null    | null                      |
| Array   | [1, 2, 3]                 |
| Date    | "2023-12-01" (as string)  |

### Restrictions
- Only double quotes for strings
- No multiline strings
- No binary/hex/positive sign
- Dates treated as strings (parsed later)

## Operator Precedence (Highest to Lowest)
- `() (grouping)`
- `not`
- `*, /, %`
- `+, -`
- `==, !=, <, >, <=, >=`
- `in, contains, startsWith, endsWith`
- `and`
- `or`

All binary operators are **left-associative**. not is prefix unary. No ambiguity errors—parsed naturally using precedence.

## Query Semantics
The interpreter:
1. Resolves the from identifier in a given context
2. Applies where filtering expression to each item
3. Evaluates select on remaining items
4. Applies result modifier (first, last, count) if provided

## Example

```silq
from people
where (Age >= 18 and Age <= 65) or (IsRetired == true and Country == "PL")
select {
  Name,
  Age,
  Country,
  Age >= 60 as NearRetirement
};
```