# Lexical Structure

## General
The following page outlines the structure and components of `SILQ` and is intended for developers interested to implement `SILQ` for other platforms.

## Syntax Grammar
The syntax grammar is used to parse the linear sequence of tokens into a nested syntax tree structure ([AST](https://w.wiki/6jCi)). It starts with the first rule that matches an entire `SILQ` script.

The documentation is using a flavour of the Backus-Naur form:
- Rules : `name → symbol`
- Terminals: `"quoted"`
- Non-Terminals: `lowercase`
- Grouping: `(Parentheses)`
- Repition: `*`
- Optional: `?`
- One or more: `+`

### Scripts
A `SILQ` **script** consists of one or more queries. Queries may be presented as a string or file. Each string/file is one script and executed as a unit.

The script executes in three steps:
1. Lexical analysis, which translates the presented input into a stream of tokens.
2. Syntactic analysis, which translates the stream of tokens into a executable structure
3. Execution, which executes every instruction in the structure and computes the return value

```bnf
script              → query* EOF ;
```

### Queries

Every query requires three parts, the `"from"` keyword, an `object` and a `where_clause`. These three core parts allow to identify the queried object and the data required. Furthermore, a query can contain a `modifier`, a `projection` and an `alias`. These extend the script capabilities beyond pure filtering and intents to make the language more useful.

```bnf
query               → modifier? "from" object where_clause projection? alias? ";" ;
```

### Modifiers

`SILQ` is providing three modifiers to modify the result set: `first`, `last`, `count`. Modifiers are optional, but only one is allowed per query.

```bnf
modifier            → "first" | "last" | "count" ;
```

### Objects
Objects allow access to data structures provided by the framework implementing `SILQ`. The object can be accesses to using an `IDENTIFIER`. Members can be accessed using the dot-notation. Members with a `private`, `protected`, `internal` or similar access modifier will be ignored where applicable.

```bnf
object              → IDENTIFIER ( ( "." member? )? )* ;
member              → IDENTIFIER ;
```

### Where Clauses
The where clause allows to write filter `condition`s using an `expression` or `function`. The `member`s used in the `condition` must be publicly accessable. The `object` member access rules apply.

```bnf
where_clause        → "where" condition ;
condition           → expression | function ;

```

### Projections
Projections allow to choose specific `members` of an `object` with the `select` keyword followed by the `member`s to be projected. The `object` member access rules apply. The projected `member`s can be access using the `alias` followed by the `member IDENTIFIER`. The `projection` will be available once an `alias` has been defined and the `query` has concluded.

```bnf
projection          → "select" "{" member ( "," member)* "}" ;
```

### Aliases

`Alias`es act in the capacity of variables and make the result of an query available for further use throughout the script. An `alias IDENTIFIER` can only be used once, it's value can not be changed.

> [!NOTE]
> While defining an `alias` is optional, `projection`s make only sense if an `alias` is defined.

```bnf
alias               → "alias" IDENTIFIER ;
```

### Function
`Function`s allow the access to functionality provided by the underlying framework. This means, that depending on framework choice, functions may behave differently. `Function`s can not be declared with `SILQ`.

```bnf
function            → IDENTIFIER "(" parameter? ")" ;
parameter           → expression ( "," expression )* ;
```

### Expression
An `expression` produces a new value by applying operators to a set of given values. The below grammar outlines the available operations as well as precedence.

```bnf
expression          → or * ;
or                  → and ( "or" and )* ;
and                 → equality ( "and" equality )* ;
equality            → comparison ( ( "!=" | "==" ) comparison )* ;
comparison          → term ( ( "<" | "<=" | ">" | ">=" ) term )* ;
term                → factor ( ( "-" | "+" ) factor )* ;
factor              → unary ( ( "/" | "*" ) unary )* ;
unary               → ( "!" | "-" ) unary | primary ;
primary             → "true" | "false" | "null" | "(" expression ")"
                      | NUMBER | STRING | IDENTIFIER | DATE 
```

## Lexical Grammar
The lexical grammer is used by the scanner to group characters into tokens. Where the syntax grammar is [context free](https://w.wiki/6jCG), the lexical grammar is [regular](https://w.wiki/DuYk) - note that there are no recursive rules.

```bnf
NUMBER              → DIGIT+ ( "." DIGIT+ )? ;
STRING              → "\"" <any char except "\""\>* "\""
IDENTIFIER          → ALPHA ( ALPHA | DIGIT )* ;
ALPHA               → [a-zA-z_] ;
DIGIT               → [0-9] ;
DATE                → "YYYY-MM-DD"
```