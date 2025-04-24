using flx.SILQ.Core;
using flx.SILQ.Models;

namespace flx.SILQ.Core.Tests;

[TestClass]
public class TokenizerTests
{
    [DataTestMethod]
    [DataRow(".", TokenType.DOT)]
    [DataRow("+", TokenType.PLUS)]
    [DataRow("-", TokenType.MINUS)]
    [DataRow(";", TokenType.SEMICOLON)]
    [DataRow("*", TokenType.STAR)]
    [DataRow("/", TokenType.SLASH)]
    [DataRow("=", TokenType.EQUAL)]
    [DataRow("<", TokenType.LESS)]
    [DataRow(">", TokenType.GREATER)]
    [DataRow("!", TokenType.BANG)]
    [DataRow("(", TokenType.LEFT_PAREN)]
    [DataRow(")", TokenType.RIGHT_PAREN)]
    public void Tokenize_SingleCharacterTokens_ReturnsExpectedTokens(string inputChar, TokenType expectedType)
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize(new[] { inputChar });

        Assert.AreEqual(2, tokens.Count); // Token + EOF
        Assert.AreEqual(expectedType, tokens[0].TokenType);
        Assert.AreEqual(inputChar, tokens[0].Lexeme);
        Assert.AreEqual(TokenType.EOF, tokens[1].TokenType);
    }

    [DataTestMethod]
    [DataRow("and", TokenType.AND)]
    [DataRow("as", TokenType.AS)]
    [DataRow("from", TokenType.FROM)]
    [DataRow("where", TokenType.WHERE)]
    [DataRow("select", TokenType.SELECT)]
    [DataRow("first", TokenType.FIRST)]
    [DataRow("last", TokenType.LAST)]
    [DataRow("count", TokenType.COUNT)]
    [DataRow("or", TokenType.OR)]
    [DataRow("true", TokenType.TRUE)]
    [DataRow("false", TokenType.FALSE)]
    [DataRow("null", TokenType.NULL)]
    [DataRow("not", TokenType.NOT)]
    [DataRow("in", TokenType.IN)]
    [DataRow("is", TokenType.IS)]
    [DataRow("like", TokenType.LIKE)]
    public void Tokenize_KeywordTokens_ReturnsExpectedTokens(string keyword, TokenType expectedType)
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize(new[] { keyword });

        Assert.AreEqual(2, tokens.Count); // Token + EOF
        Assert.AreEqual(expectedType, tokens[0].TokenType);
        Assert.AreEqual(keyword, tokens[0].Lexeme);
        Assert.AreEqual(TokenType.EOF, tokens[1].TokenType);
    }

    [DataTestMethod]
    [DataRow("123", TokenType.NUMBER, "123.0")]
    [DataRow("1.23", TokenType.NUMBER, "1.23")]
    [DataRow("9999", TokenType.NUMBER, "9999.0")]
    [DataRow("\"hello\"", TokenType.STRING, "hello")]
    [DataRow("abc", TokenType.IDENTIFIER, "abc")]
    [DataRow("_var", TokenType.IDENTIFIER, "_var")]
    [DataRow("a1b2", TokenType.IDENTIFIER, "a1b2")]
    public void Tokenize_OtherTokenTypes_ReturnsExpectedTokens(string input, TokenType expectedType, string expectedLiteral)
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize(new[] { input });

        Assert.AreEqual(2, tokens.Count); // Token + EOF
        Assert.AreEqual(expectedType, tokens[0].TokenType);
        Assert.AreEqual(expectedLiteral, tokens[0].Literal);
        Assert.AreEqual(TokenType.EOF, tokens[1].TokenType);
    }
}