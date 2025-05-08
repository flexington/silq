using System.Collections.Generic;
using System.Text;
using flx.SILQ.Errors;
using flx.SILQ.Models;

namespace flx.SILQ.Core;

public class Tokenizer
{
    private Queue<Character> _characters;
    private List<ErrorToken> _errors = new List<ErrorToken>();

    private Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
    {
        { "and", TokenType.AND },
        { "as", TokenType.AS},
        { "count", TokenType.COUNT },
        { "false", TokenType.FALSE },
        { "from", TokenType.FROM },
        { "first", TokenType.FIRST },
        { "in", TokenType.IN },
        { "is", TokenType.IS },
        { "last", TokenType.LAST },
        { "like", TokenType.LIKE },
        { "null", TokenType.NULL },
        { "not", TokenType.NOT },
        { "or", TokenType.OR },
        { "print", TokenType.PRINT },
        { "select", TokenType.SELECT },
        { "true", TokenType.TRUE },
        { "where", TokenType.WHERE }
    };

    /// <summary>
    /// Tokenizes the input lines into a list of tokens.
    /// Handles keywords, operators, numbers, identifiers, strings, and comments.
    /// Adds error tokens for unexpected characters.
    /// </summary>
    /// <param name="input">The input lines to tokenize.</param>
    /// <returns>A list of <see cref="Token"/> objects representing the tokenized input.</returns>
    public List<Token> Tokenize(string[] input)
    {
        _characters = Enqueue(input);
        List<Token> tokens = new List<Token>();

        while (_characters.Count > 0)
        {
            var c = _characters.Dequeue();
            switch (c.Value)
            {
                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    break;
                case '(':
                    tokens.Add(new Token(TokenType.LEFT_PAREN, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case ')':
                    tokens.Add(new Token(TokenType.RIGHT_PAREN, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '.':
                    tokens.Add(new Token(TokenType.DOT, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '+':
                    tokens.Add(new Token(TokenType.PLUS, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '-':
                    tokens.Add(new Token(TokenType.MINUS, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case ';':
                    tokens.Add(new Token(TokenType.SEMICOLON, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '*':
                    tokens.Add(new Token(TokenType.STAR, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '!':
                    bool matched = Match('=');
                    if (matched) tokens.Add(new Token(TokenType.BANG_EQUAL, c.Value.ToString(), null, c.Line, c.Column));
                    else tokens.Add(new Token(TokenType.BANG, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '=':
                    matched = Match('=');
                    if (matched) tokens.Add(new Token(TokenType.EQUAL_EQUAL, c.Value.ToString(), null, c.Line, c.Column));
                    else tokens.Add(new Token(TokenType.EQUAL, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '>':
                    matched = Match('=');
                    if (matched) tokens.Add(new Token(TokenType.GREATER_EQUAL, c.Value.ToString(), null, c.Line, c.Column));
                    else tokens.Add(new Token(TokenType.GREATER, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '<':
                    matched = Match('=');
                    if (matched) tokens.Add(new Token(TokenType.LESS_EQUAL, c.Value.ToString(), null, c.Line, c.Column));
                    else tokens.Add(new Token(TokenType.LESS, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '/':
                    matched = Match('/');
                    if (matched) SkipLine();
                    else tokens.Add(new Token(TokenType.SLASH, c.Value.ToString(), null, c.Line, c.Column));
                    break;
                case '"':
                    Token str = ReadString();
                    if (str != null) tokens.Add(str);
                    break;
                default:
                    if (IsDigit(c.Value))
                    {
                        tokens.Add(ToNumber(c));
                        break;
                    }
                    else if (IsAlpha(c.Value))
                    {
                        tokens.Add(Identifier(c));
                        break;
                    }
                    _errors.Add(new ErrorToken($"Unexpected character '{c.Value}'", c.Line, c.Column));
                    break;
            }

        }

        tokens.Add(new Token(TokenType.EOF, null, null, 0, 0));
        return tokens;
    }

    /// <summary>
    /// Checks if the next character in the characters <see cref="Queue{Character}"/> matches the expected value.
    /// If it matches, the character is removed from the queue.
    /// </summary>
    /// <param name="expected">The character to match against the next character in the queue.</param>
    /// <returns><c>true</c> if the next character matches and is dequeued; otherwise, <c>false</c>.</returns>
    private bool Match(char expected)
    {
        if (_characters.Count == 0) return false;
        if (_characters.Peek().Value != expected) return false;

        _characters.Dequeue();
        return true;
    }

    /// <summary>
    /// Converts an array of input strings into a queue of <see cref="Character"/> objects, preserving line and column information.
    /// Each character in the input is wrapped in a <see cref="Character"/> record with its line and column position.
    /// A newline character is enqueued at the end of each line.
    /// </summary>
    /// <param name="input">The input lines to tokenize.</param>
    /// <returns>A <see cref="Queue{Character}"/> of <see cref="Character"/> objects representing the input text with position metadata.</returns>
    private Queue<Character> Enqueue(string[] input)
    {
        Queue<Character> characters = new Queue<Character>();

        for (int l = 0; l < input.Length; l++)
        {
            var line = input[l];
            for (int c = 0; c < line.Length; c++)
            {
                var character = line[c];
                characters.Enqueue(new Character(character, l, c));
            }
            characters.Enqueue(new Character('\n', l, line.Length));
        }

        return characters;
    }

    /// <summary>
    /// Skips all remaining characters on the current line in the character queue.
    /// Advances the queue to the first character of the next line (if any).
    /// </summary>
    private void SkipLine()
    {
        var line = _characters.Peek().Line;
        while (_characters.Count > 0 && _characters.Peek().Line == line) _characters.Dequeue();
    }

    /// <summary>
    /// Reads a string literal from the character queue, handling escape sequences and unterminated strings.
    /// Consumes characters until a closing double quote is found or the end of input is reached.
    /// If the string is unterminated, an error token is added to the error list.
    /// </summary>
    /// <returns>A <see cref="Token"/> representing the string literal, or <c>null</c> if unterminated.</returns>
    private Token ReadString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        Character last = _characters.Peek();

        // Consume all characters until we find a closing quote or reach the end of the input
        while (_characters.Count > 0 && _characters.Peek().Value != '"')
        {
            last = _characters.Dequeue();
            stringBuilder.Append(last.Value);
        }

        // Check if we reached the end of the input without finding a closing quote
        if (_characters.Count == 0 || _characters.Peek().Value != '"')
        {
            _errors.Add(new ErrorToken("Unterminated string", last.Line, last.Column));
            return null;
        }

        if (_characters.Peek().Value == '"') _characters.Dequeue(); // Consume the closing quote

        return new Token(TokenType.STRING, $"\"{stringBuilder.ToString()}\"", stringBuilder.ToString(), last.Line, last.Column);
    }

    /// <summary>
    /// Determines whether the specified character is a digit (0-9).
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns><c>true</c> if the character is a digit; otherwise, <c>false</c>.</returns>
    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9' || c == '.';
    }

    /// <summary>
    /// Determines whether the specified character is an alphabetic letter (A-Z or a-z).
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns><c>true</c> if the character is an alphabetic letter; otherwise, <c>false</c>.</returns>
    private bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    }

    /// <summary>
    /// Determines whether the specified character is alphanumeric (A-Z, a-z, 0-9, or underscore).
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns><c>true</c> if the character is alphanumeric; otherwise, <c>false</c>.</returns>
    private bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    /// <summary>
    /// Consumes characters from the queue to form a number token, starting with the provided character.
    /// Continues until a non-digit character or end of line is encountered.
    /// </summary>
    /// <param name="last">The first character of the number.</param>
    /// <returns>A <see cref="Token"/> representing the parsed number.</returns>
    private Token ToNumber(Character last)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(last.Value);



        while (_characters.Count > 0 && IsDigit(_characters.Peek().Value))
        {
            if (last.Line != _characters.Peek().Line) break;
            last = _characters.Dequeue();
            sb.Append(last.Value);
        }
        return new Token(TokenType.NUMBER, sb.ToString(), double.Parse(sb.ToString()).ToString("0.0########"), last.Line, last.Column);
    }

    /// <summary>
    /// Consumes characters from the queue to form an identifier token, starting with the provided character.
    /// Continues until a non-alphanumeric character or end of line is encountered.
    /// If the resulting string matches a keyword, returns the corresponding keyword token.
    /// </summary>
    /// <param name="c">The first character of the identifier.</param>
    /// <returns>A <see cref="Token"/> representing the identifier or keyword.</returns>
    private Token Identifier(Character c)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(c.Value);
        while (_characters.Count > 0 && IsAlphaNumeric(_characters.Peek().Value))
        {
            if (c.Line != _characters.Peek().Line) break; // Break if the next character is in a different line
            c = _characters.Dequeue();
            stringBuilder.Append(c.Value);
        }

        if (_keywords.ContainsKey(stringBuilder.ToString()))
        {
            return new Token(_keywords[stringBuilder.ToString()], stringBuilder.ToString(), null, c.Line, c.Column);
        }

        return new Token(TokenType.IDENTIFIER, stringBuilder.ToString(), stringBuilder.ToString(), c.Line, c.Column);
    }
}
