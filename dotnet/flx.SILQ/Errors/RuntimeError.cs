using System;
using flx.SILQ.Models;

namespace flx.SILQ.Errors;

/// <summary>
/// Represents a runtime error that occurs during interpretation of SILQ code.
/// </summary>
public class RuntimeError : Exception
{
    private readonly Token _token;
    public Token Token => _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="RuntimeError"/> class with a token and error message.
    /// </summary>
    /// <param name="token">The token where the error occurred.</param>
    /// <param name="message">The error message.</param>
    public RuntimeError(Token token, string message) : base(message)
    {
        _token = token;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RuntimeError"/> class with a name and error message.
    /// </summary>
    /// <param name="name">The name associated with the error.</param>
    /// <param name="message">The error message.</param>
    public RuntimeError(string name, string message) : base(message)
    {
        _token = new Token(TokenType.IDENTIFIER, name, null, 0);
    }
}