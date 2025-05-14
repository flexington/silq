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
            if (Match(TokenType.SEMICOLON)) continue; // Ignore semicolon
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

        if (Match(TokenType.COUNT)) return Count();
        if (Match(TokenType.FIRST)) return First();
        if (Match(TokenType.LAST)) return Last();

        if (Match(TokenType.FROM)) return From();
        if (Match(TokenType.WHERE)) return Where();
        if (Match(TokenType.SELECT)) return Select();
        if (Match(TokenType.AS)) return As();

        throw new ParserError(Peek(), "Expect statement.");
    }

    /// <summary>
    /// Parses a last statement, which consists of an identifier followed by a semicolon.
    /// /// </summary>
    /// <returns>A <see cref="Last"/> statement containing the parsed identifier.</returns>
    private Statement Last()
    {
        return new Last();
    }

    /// <summary>
    /// Parses a first statement, which consists of an expression followed by a semicolon.
    /// </summary>
    /// <returns>A <see cref="First"/> statement containing the parsed expression.</returns>
    private Statement First()
    {
        return new First();
    }

    /// <summary>
    /// Parses a count statement, which consists of an expression followed by a semicolon.
    /// </summary>
    /// <returns>A <see cref="Count"/> statement containing the parsed expression.</returns>
    private Statement Count()
    {
        return new Count();
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

    /// <summary>
    /// Parses a from statement, which consists of an expression followed by a semicolon.
    /// </summary>
    /// <returns>A <see cref="From"/> statement containing the parsed expression.</returns>
    private Statement From()
    {
        if (Match(TokenType.IDENTIFIER))
        {
            Variable expression = Identifier();
            return new From(expression);
        }

        throw new ParserError(Peek(), "Expect identifier after 'from'.");
    }

    /// <summary>
    /// Parses a where statement, which consists of an expression followed by a semicolon.
    /// </summary>
    /// <returns>A <see cref="Where"/> statement containing the parsed expression.</returns>
    private Statement Where()
    {
        Expression expression = Expression();

        return new Where(expression);
    }

    /// <summary>
    /// Parses a select statement, which consists of an expression followed by a semicolon.
    /// </summary>
    /// <returns>A <see cref="Select"/> statement containing the parsed expression.</returns>
    private Statement Select()
    {
        List<Expression> expressions = new List<Expression>();
        Consume(TokenType.LEFT_BRACE, "Expect '{' before expression.");
        expressions.Add(Expression());

        while (!IsAtEnd() && !Check(TokenType.RIGHT_BRACE))
        {
            if (Match(TokenType.COMMA)) expressions.Add(Expression());
            else throw new ParserError(Peek(), "Expect ',' or '}' after expression.");
        }

        Consume(TokenType.RIGHT_BRACE, "Expect '}' after expression.");

        return new Select([.. expressions]);
    }

    private Statement As()
    {
        Token name = Consume(TokenType.IDENTIFIER, "Expect identifier after 'as'.");
        return new As(name);
    }
}