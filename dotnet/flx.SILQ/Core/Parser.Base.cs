using System.Collections.Generic;
using flx.SILQ.Errors;
using flx.SILQ.Models;

namespace flx.SILQ.Core;

/// <summary>
/// Provides utility methods for parsing tokens into expressions and statements in the SILQ language.
/// </summary>
public partial class Parser
{
    private int _current = 0;
    private List<Token> _tokens;

    /// <summary>
    /// Attempts to match the current token against a set of token types. If a match is found, advances the parser.
    /// </summary>
    /// <param name="types">The token types to match against.</param>
    /// <returns>True if a match is found and the parser advances; otherwise, false.</returns>
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

    /// <summary>
    /// Advances the parser to the next token and returns the previous token.
    /// </summary>
    /// <returns>The previous token before advancing.</returns>
    private Token Advance()
    {
        if (!IsAtEnd()) _current++;
        return Previous();
    }

    /// <summary>
    /// Returns the most recently consumed token.
    /// </summary>
    /// <returns>The previous token.</returns>
    private Token Previous()
    {
        return _tokens[_current - 1];
    }

    /// <summary>
    /// Checks if the parser has reached the end of the token list.
    /// </summary>
    /// <returns>True if at the end; otherwise, false.</returns>
    private bool IsAtEnd()
    {
        return Peek().TokenType == TokenType.EOF;
    }

    /// <summary>
    /// Checks if the current token matches the specified type.
    /// </summary>
    /// <param name="type">The token type to check.</param>
    /// <returns>True if the current token matches; otherwise, false.</returns>
    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().TokenType == type;
    }

    /// <summary>
    /// Returns the current token without advancing the parser.
    /// </summary>
    /// <returns>The current token.</returns>
    private Token Peek()
    {
        return _tokens[_current];
    }

    /// <summary>
    /// Consumes the current token if it matches the expected type, otherwise throws a <see cref="ParserError"/>.
    /// </summary>
    /// <param name="token">The expected token type.</param>
    /// <param name="message">The error message if the token does not match.</param>
    /// <returns>The consumed token.</returns>
    private Token Consume(TokenType token, string message)
    {
        if (Check(token)) return Advance();
        else throw new ParserError(Peek(), message);
    }
}