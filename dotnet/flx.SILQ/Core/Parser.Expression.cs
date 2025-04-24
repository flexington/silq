using System.Collections.Generic;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;

namespace flx.SILQ.Core;

public partial class Parser
{
    private int _current = 0;
    private List<Token> _tokens;

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

    private Expression Expression()
    {
        return Equality();
    }

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

        if (Match(TokenType.IDENTIFIER)) throw new ParserError(Peek(), "Identifier not implemented yet");

        throw new ParserError(Peek(), "Expected expression");

    }

    private bool Match(params TokenType[] types)
    {
        foreach (var type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private Token Advance()
    {
        if (!IsAtEnd()) _current++;
        return Previous();
    }

    private Token Previous()
    {
        return _tokens[_current - 1];
    }

    private bool IsAtEnd()
    {
        return Peek().TokenType == TokenType.EOF;
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().TokenType == type;
    }

    private Token Peek()
    {
        return _tokens[_current];
    }
}