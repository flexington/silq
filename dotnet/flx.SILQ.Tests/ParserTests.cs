using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;

namespace flx.SILQ.Core.Tests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void ParseExpression_WhenString_ReturnsLiteral()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.STRING, "\"Hello World\"", "Hello World", 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Literal));
        Assert.AreEqual("Hello World", ((Literal)expression).Value);
    }

    [TestMethod]
    public void ParseExpression_WhenNumber_ReturnsLiteral()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "123", 123.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Literal));
        Assert.AreEqual(123.0, ((Literal)expression).Value);
    }

    [TestMethod]
    public void ParseExpression_WhenTrue_ReturnsLiteral()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.TRUE, "true", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Literal));
        Assert.AreEqual(true, ((Literal)expression).Value);
    }

    [TestMethod]
    public void ParseExpression_WhenFalse_ReturnsLiteral()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.FALSE, "false", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Literal));
        Assert.AreEqual(false, ((Literal)expression).Value);
    }

    [TestMethod]
    public void ParseExpression_WhenNull_ReturnsLiteral()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NULL, "null", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Literal));
        Assert.AreEqual(null, ((Literal)expression).Value);
    }

    [TestMethod]
    public void ParseExpression_WhenEquality_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.EQUAL_EQUAL, "==", null, 1),
            new Token(TokenType.NUMBER, "58", 58.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Binary));
    }

    [TestMethod]
    public void ParseExpression_WhenNotEqual_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.BANG_EQUAL, "!=", null, 1),
            new Token(TokenType.NUMBER, "58", 58.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Binary));
    }

    [TestMethod]
    public void ParseExpression_WhenGreater_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.GREATER, ">", null, 1),
            new Token(TokenType.NUMBER, "58", 58.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Binary));
    }

    [TestMethod]
    public void ParseExpression_WhenLess_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.LESS, "<", null, 1),
            new Token(TokenType.NUMBER, "58", 58.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Binary));
    }

    [TestMethod]
    public void ParseExpression_WhenGreaterEqual_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.GREATER_EQUAL, ">=", null, 1),
            new Token(TokenType.NUMBER, "58", 58.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Binary));
    }

    [TestMethod]
    public void ParseExpression_WhenLessEqual_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.LESS_EQUAL, "<=", null, 1),
            new Token(TokenType.NUMBER, "58", 58.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Binary));
    }

    [TestMethod]
    public void ParseExpression_WhenPlus_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.PLUS, "+", null, 1),
            new Token(TokenType.NUMBER, "58", 58.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Binary));
    }

    [TestMethod]
    public void ParseExpression_WhenMinus_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.MINUS, "-", null, 1),
            new Token(TokenType.NUMBER, "58", 58.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Binary));
    }

    [TestMethod]
    public void ParseExpression_WhenAnd_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.TRUE, "true", null, 1),
            new Token(TokenType.AND, "and", null, 1),
            new Token(TokenType.FALSE, "false", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Literal));
    }

    [TestMethod]
    public void ParseExpression_WhenOr_ReturnsBinaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.TRUE, "true", null, 1),
            new Token(TokenType.OR, "or", null, 1),
            new Token(TokenType.FALSE, "false", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Literal));
    }

    [TestMethod]
    public void ParseExpression_WhenNot_ReturnsUnaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.BANG, "!", null, 1),
            new Token(TokenType.TRUE, "true", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Unary));
    }

    [TestMethod]
    public void ParseExpression_WhenMinus_ReturnsUnaryExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.MINUS, "-", null, 1),
            new Token(TokenType.NUMBER, "66", 66.0, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Unary));
    }

    [TestMethod]
    public void ParseExpression_WhenIdentifier_ThrowsParserError()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.IDENTIFIER, "myVar", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act & Assert
        var parser = new Parser();
        Assert.ThrowsException<ParserError>(() => parser.ParseExpression(tokens));
    }

    [TestMethod]
    public void ParseExpression_WhenInvalidToken_ThrowsParserError()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act & Assert
        var parser = new Parser();
        Assert.ThrowsException<ParserError>(() => parser.ParseExpression(tokens));
    }

    [TestMethod]
    public void ParseExpression_WhenEmptyTokenList_ThrowsParserError()
    {
        // Arrange
        var tokens = new List<Token>();

        // Act & Assert
        var parser = new Parser();
        Assert.ThrowsException<ParserError>(() => parser.ParseExpression(tokens));
    }
}