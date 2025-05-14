using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;

namespace flx.SILQ.Core;

/// <summary>
/// Provides expression parsing logic for the SILQ language, supporting operator precedence and grouping.
/// </summary>
public partial class Parser
{
    /// <summary>
    /// Parses a list of tokens into an expression AST.
    /// </summary>
    /// <param name="tokens">The list of tokens to parse.</param>
    /// <returns>The root <see cref="Expression"/> node.</returns>
    /// <exception cref="ParserError">Thrown if the token list is empty or parsing fails.</exception>
    public Expression ParseExpression(List<Token> tokens)
    {
        if (tokens == null || tokens.Count == 0)
            throw new ParserError(new Token(TokenType.EOF, "", null, 0), "No tokens to parse");

        _tokens = tokens;
        _current = 0;

        try
        {
            return Expression();
        }
        catch (ParserError)
        {
            throw;
        }
    }

    /// <summary>
    /// Parses the highest-level expression (entry point for precedence parsing).
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    private Expression Expression()
    {
        return Or();
    }

    /// <summary>
    /// Parses logical OR expressions.
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    private Expression Or(){
        Expression expression = And();

        while (Match(TokenType.OR))
        {
            var operatorToken = Previous();
            var right = And();
            expression = new Logical(expression, operatorToken, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses logical AND expressions.
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    private Expression And()
    {
        Expression expression = Equality();

        while (Match(TokenType.AND))
        {
            var operatorToken = Previous();
            var right = Equality();
            expression = new Logical(expression, operatorToken, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses equality expressions (==, !=).
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    private Expression Equality()
    {
        Expression expression = Comparison();

        while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            var operatorToken = Previous();
            var right = Comparison();
            expression = new Binary(expression, operatorToken, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses comparison expressions (>, >=, <, <=).
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    private Expression Comparison()
    {
        Expression expression = Term();

        while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            var operatorToken = Previous();
            var right = Term();
            expression = new Binary(expression, operatorToken, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses term expressions (+, -).
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    private Expression Term()
    {
        Expression expression = Factor();

        while (Match(TokenType.MINUS, TokenType.PLUS))
        {
            var operatorToken = Previous();
            var right = Factor();
            expression = new Binary(expression, operatorToken, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses factor expressions (*, /).
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    private Expression Factor()
    {
        Expression expression = Unary();

        while (Match(TokenType.SLASH, TokenType.STAR))
        {
            var operatorToken = Previous();
            var right = Unary();
            expression = new Binary(expression, operatorToken, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses unary expressions (!, -).
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    private Expression Unary()
    {
        if (Match(TokenType.BANG, TokenType.MINUS))
        {
            var operatorToken = Previous();
            var right = Unary();
            return new Unary(operatorToken, right);
        }

        return Primary();
    }

    /// <summary>
    /// Parses primary expressions: literals, grouping, and identifiers.
    /// </summary>
    /// <returns>The parsed <see cref="Expression"/>.</returns>
    /// <exception cref="ParserError">Thrown if the token does not match a valid primary expression.</exception>
    private Expression Primary()
    {
        if (Match(TokenType.FALSE)) return new Literal(false);
        if (Match(TokenType.TRUE)) return new Literal(true);
        if (Match(TokenType.NULL)) return new Literal(null);

        if (Match(TokenType.NUMBER, TokenType.STRING))
        {
            var previous = Previous();
            if (previous.TokenType == TokenType.NUMBER) return new Literal(double.Parse(previous.Lexeme));
            if (previous.TokenType == TokenType.STRING) return new Literal(previous.Literal);
        }

        if (Match(TokenType.IDENTIFIER))
        {
            if (Peek().TokenType == TokenType.LEFT_PAREN) return Function();
            else return Identifier();
        }

        if (Match(TokenType.LEFT_PAREN))
        {
            var expression = Expression();
            if (!Match(TokenType.RIGHT_PAREN)) throw new ParserError(Peek(), "Expected ')' after expression");
            return new Grouping(expression);
        }

        throw new ParserError(Peek(), "Expected expression");
    }

    private Variable Identifier()
    {
        var token = Previous();

        while (Match(TokenType.DOT))
        {
            if (Match(TokenType.IDENTIFIER))
            {
                Variable right = Identifier();
                return new Variable(token, right);
            }
            else
            {
                throw new ParserError(Peek(), "Expected identifier after '.'");
            }
        }


        return new Variable(token, null);
    }

    private Function Function()
    {
        var args = new List<Expression>();

        var name = Previous();

        Consume(TokenType.LEFT_PAREN, "Expected '(' after function name");

        args.Add(Expression());

        while (Match(TokenType.COMMA))
        {
            args.Add(Expression());
        }

        Consume(TokenType.RIGHT_PAREN, "Expected ')' after function arguments");

        return new Function(name, args);
    }
}