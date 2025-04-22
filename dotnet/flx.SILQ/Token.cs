namespace flx.SILQ;

/// <summary>
/// Represents a token identified by the SILQ tokenizer, including its type, lexeme, literal value, and line number.
/// </summary>
/// <param name="TokenType">The type of the token.</param>
/// <param name="Lexeme">The string representation of the token as found in the source code.</param>
/// <param name="Literal">The literal value of the token, if applicable.</param>
/// <param name="Line">The line number where the token was found.</param>
public record Token(TokenType TokenType, string Lexeme, object Literal = null, int Line = -1, int Column = -1)
{
    /// <summary>
    /// Returns a formatted string representation of the token, including its type, lexeme, and literal value.
    /// </summary>
    /// <returns>A string in the format: TokenType Lexeme Literal</returns>
    public override string ToString()
    {
        return $"{TokenType} {Lexeme} {Literal}";
    }
}
