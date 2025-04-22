using System.Collections.Generic;
using System.Text;
using flx.SILQ.Errors;

namespace flx.SILQ;

public class Tokenizer
{
    private Queue<Character> _characters;
    private List<Token> _tokens = new List<Token>();
    private List<ErrorToken> _errors = new List<ErrorToken>();

    private Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
    {
        { "from", TokenType.FROM },
        { "where", TokenType.WHERE },
        { "select", TokenType.SELECT },
        { "first", TokenType.FIRST },
        { "last", TokenType.LAST },
        { "count", TokenType.COUNT },
        { "and", TokenType.AND },
        { "or", TokenType.OR },
        { "true", TokenType.TRUE },
        { "false", TokenType.FALSE },
        { "null", TokenType.NULL },
        { "not", TokenType.NOT },
        { "in", TokenType.IN },
        { "is", TokenType.IS },
        { "like", TokenType.LIKE }
    };

    public Tokenizer(string[] input)
    {
        _characters = Enqueue(input);
    }


    public List<Token> Tokenize(string input)
    {
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
                case '.':
                    tokens.Add(new Token(TokenType.DOT, c.ToString(), null, c.Line, c.Column));
                    break;
                case '+':
                    tokens.Add(new Token(TokenType.PLUS, c.ToString(), null, c.Line, c.Column));
                    break;
                case '-':
                    tokens.Add(new Token(TokenType.MINUS, c.ToString(), null, c.Line, c.Column));
                    break;
                case ';':
                    tokens.Add(new Token(TokenType.SEMICOLON, c.ToString(), null, c.Line, c.Column));
                    break;
                case '*':
                    tokens.Add(new Token(TokenType.STAR, c.ToString(), null, c.Line, c.Column));
                    break;
                case '!':
                    bool matched = Match('=');
                    if (matched) tokens.Add(new Token(TokenType.BANG_EQUAL, c.ToString(), null, c.Line, c.Column));
                    else tokens.Add(new Token(TokenType.BANG, c.ToString(), null, c.Line, c.Column));
                    break;
                case '=':
                    matched = Match('=');
                    if (matched) tokens.Add(new Token(TokenType.EQUAL_EQUAL, c.ToString(), null, c.Line, c.Column));
                    else tokens.Add(new Token(TokenType.EQUAL, c.ToString(), null, c.Line, c.Column));
                    break;
                case '>':
                    matched = Match('=');
                    if (matched) tokens.Add(new Token(TokenType.GREATER_EQUAL, c.ToString(), null, c.Line, c.Column));
                    else tokens.Add(new Token(TokenType.GREATER, c.ToString(), null, c.Line, c.Column));
                    break;
                case '<':
                    matched = Match('=');
                    if (matched) tokens.Add(new Token(TokenType.LESS_EQUAL, c.ToString(), null, c.Line, c.Column));
                    else tokens.Add(new Token(TokenType.LESS, c.ToString(), null, c.Line, c.Column));
                    break;
                case '/':
                    matched = Match('/');
                    if (matched) SkipLine();
                    else tokens.Add(new Token(TokenType.SLASH, c.ToString(), null, c.Line, c.Column));
                    break;
                case '"':
                    Token str = ReadString();
                    if (str != null) tokens.Add(str);
                    break;
                default:
                    // Check if the character is a digit
                    // Check if the character is an identifier
                    break;

            }

        }
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
        if (_characters.Count == 0 && _characters.Peek().Value != '"')
        {
            _errors.Add(new ErrorToken("Unterminated string", last.Line, last.Column));
            return null;
        }

        if (_characters.Peek().Value == '"') _characters.Dequeue(); // Consume the closing quote

        return new Token(TokenType.STRING, $"\"{stringBuilder.ToString()}\"", stringBuilder.ToString(), last.Line, last.Column);
    }
}
