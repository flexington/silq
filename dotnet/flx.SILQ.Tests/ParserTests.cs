using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;
using flx.SILQ.Statements;

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
    public void ParseExpression_WhenIdentifier_ReturnsIdentifierExpression()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.IDENTIFIER, "myVar", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Variable));
        Assert.AreEqual("myVar", ((Variable)expression).Name.Lexeme);
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

    [TestMethod]
    public void ParseExpression_WhenGrouping_ReturnsGroupingExpression()
    {
        // Arrange: (123)
        var tokens = new List<Token>
        {
            new Token(TokenType.LEFT_PAREN, "(", null, 1),
            new Token(TokenType.NUMBER, "123", 123.0, 1),
            new Token(TokenType.RIGHT_PAREN, ")", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Grouping));
        var grouping = (Grouping)expression;
        Assert.IsInstanceOfType(grouping.Expression, typeof(Literal));
        Assert.AreEqual(123.0, ((Literal)grouping.Expression).Value);
    }

    [TestMethod]
    public void ParseExpression_WhenNestedGrouping_ReturnsGroupingExpression()
    {
        // Arrange: ((true))
        var tokens = new List<Token>
        {
            new Token(TokenType.LEFT_PAREN, "(", null, 1),
            new Token(TokenType.LEFT_PAREN, "(", null, 1),
            new Token(TokenType.TRUE, "true", null, 1),
            new Token(TokenType.RIGHT_PAREN, ")", null, 1),
            new Token(TokenType.RIGHT_PAREN, ")", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Grouping));
        var outer = (Grouping)expression;
        Assert.IsInstanceOfType(outer.Expression, typeof(Grouping));
        var inner = (Grouping)outer.Expression;
        Assert.IsInstanceOfType(inner.Expression, typeof(Literal));
        Assert.AreEqual(true, ((Literal)inner.Expression).Value);
    }

    [TestMethod]
    public void ParseExpression_WhenGroupingWithBinary_ReturnsGroupingExpression()
    {
        // Arrange: (1 + 2)
        var tokens = new List<Token>
        {
            new Token(TokenType.LEFT_PAREN, "(", null, 1),
            new Token(TokenType.NUMBER, "1", 1.0, 1),
            new Token(TokenType.PLUS, "+", null, 1),
            new Token(TokenType.NUMBER, "2", 2.0, 1),
            new Token(TokenType.RIGHT_PAREN, ")", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);
        Assert.IsInstanceOfType(expression, typeof(Grouping));
        var grouping = (Grouping)expression;
        Assert.IsInstanceOfType(grouping.Expression, typeof(Binary));
        var binary = (Binary)grouping.Expression;
        Assert.IsInstanceOfType(binary.Left, typeof(Literal));
        Assert.IsInstanceOfType(binary.Right, typeof(Literal));
        Assert.AreEqual(1.0, ((Literal)binary.Left).Value);
        Assert.AreEqual(2.0, ((Literal)binary.Right).Value);
    }

    [TestMethod]
    public void ParseExpression_WhenFunctionWithSingleArgument_ReturnsFunctionCall()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.IDENTIFIER, "myFunc", null, 1),
            new Token(TokenType.LEFT_PAREN, "(", null, 1),
            new Token(TokenType.NUMBER, "123", 123.0, 1),
            new Token(TokenType.RIGHT_PAREN, ")", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

        // Assert
        Assert.IsNotNull(expression);

        Assert.IsInstanceOfType(expression, typeof(Expressions.Function));
        var function = (Expressions.Function)expression;
        Assert.IsNotNull(function.Name);
        Assert.AreEqual("myFunc", function.Name.Lexeme);
        Assert.IsNotNull(function.Arguments);
        Assert.AreEqual(1, function.Arguments.Count);

        Assert.IsInstanceOfType(function.Arguments[0], typeof(Literal));
        var value = (Literal)function.Arguments[0];
        Assert.AreEqual(123.0, value.Value);
    }

    [TestMethod]
    public void ParseExpression_WhenFunctionWithMultipleArguments_ReturnsFunctionCall()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.IDENTIFIER, "myFunc", null, 1),
            new Token(TokenType.LEFT_PAREN, "(", null, 1),
            new Token(TokenType.STRING, "\"param\'", "param", 1),
            new Token(TokenType.COMMA, ",", null, 1),
            new Token(TokenType.NUMBER, "456", 456.0, 1),
            new Token(TokenType.RIGHT_PAREN, ")", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var expression = parser.ParseExpression(tokens);

         // Assert
        Assert.IsNotNull(expression);

        Assert.IsInstanceOfType(expression, typeof(Expressions.Function));
        var function = (Expressions.Function)expression;
        Assert.IsNotNull(function.Name);
        Assert.AreEqual("myFunc", function.Name.Lexeme);
        Assert.IsNotNull(function.Arguments);
        Assert.AreEqual(2, function.Arguments.Count);

        Assert.IsInstanceOfType(function.Arguments[0], typeof(Literal));
        var value_1 = (Literal)function.Arguments[0];
        Assert.AreEqual("param", value_1.Value);

        Assert.IsInstanceOfType(function.Arguments[1], typeof(Literal));
        var value_2 = (Literal)function.Arguments[1];
        Assert.AreEqual(456.0, value_2.Value);
    }

    [TestMethod]
    public void ParseStatement_WhenPrintStatement_ReturnsPrintStatement()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.PRINT, "print", null, 1),
            new Token(TokenType.STRING, "\"Hello World\"", "Hello World", 1),
            new Token(TokenType.SEMICOLON, ";", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var statement = parser.ParseStatements(tokens);

        // Assert
        Assert.IsNotNull(statement);
        Assert.IsInstanceOfType(statement.First(), typeof(Print));
        Assert.IsInstanceOfType(((Print)statement.First()).Expression, typeof(Literal));
        Assert.AreEqual("Hello World", ((Literal)((Print)statement.First()).Expression).Value);
    }

    [TestMethod]
    public void ParseStatement_WhenFromStatement_ReturnsFromStatement()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.FROM, "from", null, 1),
            new Token(TokenType.IDENTIFIER, "myVar", null, 1),
            new Token(TokenType.SEMICOLON, ";", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var statement = parser.ParseStatements(tokens);

        // Assert
        Assert.IsNotNull(statement);
        Assert.IsNotNull(statement.First());

        Assert.IsInstanceOfType(statement.First(), typeof(From));
        Assert.IsInstanceOfType(((From)statement.First()).Property, typeof(Variable));
    }

    [TestMethod]
    public void ParseStatement_WhenWhereStatement_ReturnsWhereStatement()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.WHERE, "where", null, 1),
            new Token(TokenType.IDENTIFIER, "myVar", null, 1),
            new Token(TokenType.EQUAL_EQUAL, "==", null, 1),
            new Token(TokenType.NUMBER, "123", 123.0, 1),
            new Token(TokenType.SEMICOLON, ";", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };
        var parser = new Parser();

        // Act
        var statement = parser.ParseStatements(tokens);

        // Assert
        Assert.IsNotNull(statement);
        Assert.IsNotNull(statement.First());
        Assert.IsInstanceOfType(statement.First(), typeof(Where));
        Assert.IsNotNull(((Where)statement.First()).Expression);
    }

    [TestMethod]
    public void ParseStatement_WhenSelectStatementWithSingleMember_ReturnsSelectStatement()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.SELECT, "select", null, 1),
            new Token(TokenType.LEFT_BRACE, "{", null, 1),
            new Token(TokenType.IDENTIFIER, "myVar", null, 1),
            new Token(TokenType.RIGHT_BRACE, "}", null, 1),
            new Token(TokenType.SEMICOLON, ";", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var statement = parser.ParseStatements(tokens);

        // Assert
        Assert.IsNotNull(statement);
        Assert.IsInstanceOfType(statement.First(), typeof(Select));

        var selectStatement = (Select)statement.First();
        Assert.IsNotNull(selectStatement.Expressions);
        Assert.AreEqual(1, selectStatement.Expressions.Length);
        Assert.IsInstanceOfType(selectStatement.Expressions[0], typeof(Variable));
    }

    [TestMethod]
    public void ParseStatement_WhenSelectStatementWithMultipleMembers_ReturnsSelectStatement()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.SELECT, "select", null, 1),
            new Token(TokenType.LEFT_BRACE, "{", null, 1),
            new Token(TokenType.IDENTIFIER, "myVar", null, 1),
            new Token(TokenType.COMMA, ",", null, 1),
            new Token(TokenType.IDENTIFIER, "myVar2", null, 1),
            new Token(TokenType.RIGHT_BRACE, "}", null, 1),
            new Token(TokenType.SEMICOLON, ";", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };
        var parser = new Parser();

        // Act
        var statement = parser.ParseStatements(tokens);

        // Assert
        Assert.IsNotNull(statement);
        Assert.IsInstanceOfType(statement.First(), typeof(Select));

        var selectStatement = (Select)statement.First();
        Assert.IsNotNull(selectStatement.Expressions);
        Assert.AreEqual(2, selectStatement.Expressions.Length);
        Assert.IsInstanceOfType(selectStatement.Expressions[0], typeof(Variable));
        Assert.IsInstanceOfType(selectStatement.Expressions[1], typeof(Variable));
    }

    [TestMethod]
    public void ParseStatement_WhenAsStatement_ReturnsAsStatement()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.AS, "as", null, 1),
            new Token(TokenType.IDENTIFIER, "myVar", null, 1),
            new Token(TokenType.SEMICOLON, ";", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var statements = parser.ParseStatements(tokens);

        // Assert
        Assert.IsNotNull(statements);
        Assert.IsInstanceOfType(statements.First(), typeof(As));

        var asStatement = (As)statements.First();
        Assert.IsNotNull(asStatement.Name);
        Assert.AreEqual("myVar", asStatement.Name.Lexeme);
    }

    [TestMethod]
    public void ParseStatement_WhenWhereStatementWithMemberAccess_ReturnsWhereStatement()
    {
        // Arrange
        var tokens = new List<Token>
        {
            new Token(TokenType.WHERE, "where", null, 1),
            new Token(TokenType.IDENTIFIER, "myVar", null, 1),
            new Token(TokenType.DOT, ".", null, 1),
            new Token(TokenType.IDENTIFIER, "myMember", null, 1),
            new Token(TokenType.EQUAL_EQUAL, "==", null, 1),
            new Token(TokenType.NUMBER, "123", 123.0, 1),
            new Token(TokenType.SEMICOLON, ";", null, 1),
            new Token(TokenType.EOF, null, null, 1)
        };

        // Act
        var parser = new Parser();
        var statement = parser.ParseStatements(tokens);

        // Assert
        Assert.IsNotNull(statement);
        Assert.IsInstanceOfType(statement.First(), typeof(Where));

        var whereStatement = (Where)statement.First();
        Assert.IsNotNull(whereStatement.Expression);

        var left = ((Binary)whereStatement.Expression).Left;
        Assert.IsNotNull(left);
        Assert.IsInstanceOfType(left, typeof(Variable));

        var variable = (Variable)left;
        Assert.AreEqual("myVar", variable.Name.Lexeme);
        Assert.IsNotNull(variable.Member);
        Assert.IsInstanceOfType(variable.Member, typeof(Variable));

        var member = (Variable)variable.Member;
        Assert.AreEqual("myMember", member.Name.Lexeme);
        Assert.IsNull(member.Member);
    }
}