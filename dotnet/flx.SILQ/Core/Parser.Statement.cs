using System;
using System.Collections.Generic;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;
using flx.SILQ.Statements;

namespace flx.SILQ.Core;

/// <summary>
/// Provides statement parsing logic for the SILQ language, including print statements.
/// </summary>
public partial class Parser
{
    /// <summary>
    /// Parses a list of tokens into a list of statements.
    /// </summary>
    /// <param name="tokens">The list of tokens to parse.</param>
    /// <returns>A list of parsed <see cref="Statement"/> objects.</returns>
    public List<Statement> ParseStatements(List<Token> tokens)
    {
        if (tokens == null || tokens.Count == 0)
            throw new ParserError(new Token(TokenType.EOF, "", null, 0), "No tokens to parse");

        _tokens = tokens;
        _current = 0;

        List<Statement> statements = new List<Statement>();

        while (!IsAtEnd())
        {
            statements.Add(Statement());
        }

        return statements;
    }

    /// <summary>
    /// Parses a single statement. Currently only supports print statements.
    /// </summary>
    /// <returns>The parsed <see cref="Statement"/>.</returns>
    private Statement Statement()
    {
        if (Match(TokenType.PRINT)) return Print();

        throw new ParserError(Peek(), "Expect statement.");
    }

    /// <summary>
    /// Parses a print statement, which consists of an expression followed by a semicolon.
    /// </summary>
    /// <returns>A <see cref="Print"/> statement containing the parsed expression.</returns>
    private Statement Print()
    {
        Expression expression = Expression();
        Consume(TokenType.SEMICOLON, "Expect ';' after value.");
        return new Print(expression);
    }
}