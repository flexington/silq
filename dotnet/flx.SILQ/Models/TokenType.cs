namespace flx.SILQ.Models;

/// <summary>
/// Represents the different types of tokens that can be identified by the SILQ tokenizer.
/// </summary>
public enum TokenType
{
    /// <summary>
    /// Logical AND
    /// </summary>
    AND,

    /// <summary>
    /// 'as' keyword
    /// </summary>
    AS,

    /// <summary>
    /// Bang '!'
    /// </summary>
    BANG,

    /// <summary>
    /// Bang equal '!='
    /// </summary>
    BANG_EQUAL,

    /// <summary>
    /// Comma ','
    /// </summary>
    COMMA,

    /// <summary>
    /// 'count' keyword
    /// </summary>
    COUNT,

    /// <summary>
    /// Dot '.'
    /// </summary>
    DOT,

    /// <summary>
    /// End of file/input
    /// </summary>
    EOF,

    /// <summary>
    /// Equal '='
    /// </summary>
    EQUAL,

    /// <summary>
    /// Equal equal '=='
    /// </summary>
    EQUAL_EQUAL,

    /// <summary>
    /// Boolean literal 'false'
    /// </summary>
    FALSE,

    /// <summary>
    /// 'first' keyword
    /// </summary>
    FIRST,

    /// <summary>
    /// 'from' keyword
    /// </summary>
    FROM,

    /// <summary>
    /// Greater '>'
    /// </summary>
    GREATER,

    /// <summary>
    /// Greater equal '>='
    /// </summary>
    GREATER_EQUAL,

    /// <summary>
    /// 'in' keyword
    /// </summary>
    IN,

    /// <summary>
    /// Identifier (variable or function name)
    /// </summary>
    IDENTIFIER,

    /// <summary>
    /// 'is' keyword
    /// </summary>
    IS,

    /// <summary>
    /// 'last' keyword
    /// </summary>
    LAST,

    /// <summary>
    /// Left brace '{'
    /// </summary>
    LEFT_BRACE,

    /// <summary>
    /// Left brace '('
    /// </summary>
    LEFT_PAREN,

    /// <summary>
    /// Less '<'
    /// </summary>
    LESS,

    /// <summary>
    /// Less equal '<='
    /// </summary>
    LESS_EQUAL,

    /// <summary>
    /// 'like' keyword
    /// </summary>
    LIKE,

    /// <summary>
    /// Minus '-'
    /// </summary>
    MINUS,

    /// <summary>
    /// 'not' keyword
    /// </summary>
    NOT,

    /// <summary>
    /// Null literal
    /// </summary>
    NULL,

    /// <summary>
    /// Number literal
    /// </summary>
    NUMBER,

    /// <summary>
    /// Logical OR
    /// </summary>
    OR,

    /// <summary>
    /// Plus '+'
    /// </summary>
    PLUS,

    /// <summary>
    /// 'print' keyword
    /// </summary>
    PRINT,

    /// <summary>
    /// Right brace '}'
    /// </summary>
    RIGHT_BRACE,

    /// <summary>
    /// Right brace ')'
    /// </summary>
    RIGHT_PAREN,

    /// <summary>
    /// 'select' keyword
    /// </summary>
    SELECT,

    /// <summary>
    /// Semicolon ';'
    /// </summary>
    SEMICOLON,

    /// <summary>
    /// Slash '/'
    /// </summary>
    SLASH,

    /// <summary>
    /// Star '*'
    /// </summary>
    STAR,

    /// <summary>
    /// String literal
    /// </summary>
    STRING,

    /// <summary>
    /// Boolean literal 'true'
    /// </summary>
    TRUE,

    /// <summary>
    /// 'where' keyword
    /// </summary>
    WHERE
}