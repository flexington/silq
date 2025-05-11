using System;
using System.Collections.Generic;
using System.IO;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;
using flx.SILQ.Statements;

namespace flx.SILQ.Core.Tests;

[TestClass]
public class InterpreterTests
{
    [TestMethod]
    public void Interpret_WhenAddition_ReturnsResult()
    {
        // Arrange
        var left = new Literal(2.0);
        var right = new Literal(3.0);
        var op = new Token(TokenType.PLUS, "+", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("5", result);
    }

    [TestMethod]
    public void Interpret_WhenNumberHasZeroDecimal_ReturnWithoutDecimal()
    {
        // Arrange
        var expr = new Literal(42.0);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("42", result);
    }

    [TestMethod]
    public void Interpret_WhenNegativeNumber_ReturnsResult()
    {
        // Arrange
        var number = new Literal(7.0);
        var op = new Token(TokenType.MINUS, "-", null, 1);
        var expr = new Unary(op, number);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("-7", result);
    }

    [TestMethod]
    public void Interpret_WhenNotTrue_ReturnsFalse()
    {
        // Arrange
        var value = new Literal(true);
        var op = new Token(TokenType.BANG, "!", null, 1);
        var expr = new Unary(op, value);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("false", result);
    }

    [TestMethod]
    public void Interpret_WhenNotFalse_ReturnsTrue()
    {
        // Arrange
        var value = new Literal(false);
        var op = new Token(TokenType.BANG, "!", null, 1);
        var expr = new Unary(op, value);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("true", result);
    }

    [TestMethod]
    public void Interpret_WhenNotNumber_ReturnsFalse()
    {
        // Arrange
        var value = new Literal(0.0);
        var op = new Token(TokenType.BANG, "!", null, 1);
        var expr = new Unary(op, value);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("false", result);
    }

    [TestMethod]
    public void Interpret_WhenStringConcatenation_ReturnsResult()
    {
        // Arrange
        var left = new Literal("Hello, ");
        var right = new Literal("World!");
        var op = new Token(TokenType.PLUS, "+", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("Hello, World!", result);
    }

    [TestMethod]
    public void Interpret_WhenGreaterThan_ReturnsTrue()
    {
        // Arrange
        var left = new Literal(5.0);
        var right = new Literal(2.0);
        var op = new Token(TokenType.GREATER, ">", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("true", result);
    }

    [TestMethod]
    public void Interpret_WhenGreaterThanEqual_ReturnsTrue()
    {
        // Arrange
        var left = new Literal(5.0);
        var right = new Literal(5.0);
        var op = new Token(TokenType.GREATER_EQUAL, ">=", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("true", result);
    }

    [TestMethod]
    public void Interpret_WhenComplexGreaterThanEqual_ReturnsTrue()
    {
        // Arrange
        var left = new Binary(new Literal(2.0), new Token(TokenType.PLUS, "+", null, 1), new Literal(3.0)); // 2 + 3
        var right = new Literal(5.0);
        var op = new Token(TokenType.GREATER_EQUAL, ">=", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("true", result);
    }

    [TestMethod]
    public void Interpret_WhenStringEqual_ReturnsFalse()
    {
        // Arrange
        var left = new Literal("foo");
        var right = new Literal("bar");
        var op = new Token(TokenType.EQUAL_EQUAL, "==", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("false", result);
    }

    [TestMethod]
    public void Interpret_WhenStringNotEqual_ReturnsTrue()
    {
        // Arrange
        var left = new Literal("foo");
        var right = new Literal("bar");
        var op = new Token(TokenType.BANG_EQUAL, "!=", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("true", result);
    }

    [TestMethod]
    public void Interpret_WhenStringEqual_ReturnsTrue()
    {
        // Arrange
        var left = new Literal("foo");
        var right = new Literal("foo");
        var op = new Token(TokenType.EQUAL_EQUAL, "==", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("true", result);
    }

    [TestMethod]
    public void Interpret_WhenNumberEqualString_ReturnsFalse()
    {
        // Arrange
        var left = new Literal(42.0);
        var right = new Literal("42");
        var op = new Token(TokenType.EQUAL_EQUAL, "==", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("false", result);
    }

    [TestMethod]
    public void Interpret_WhenStringPlusBoolean_ReturnsRuntimeError()
    {
        // Arrange
        var left = new Literal("foo");
        var right = new Literal(true);
        var op = new Token(TokenType.PLUS, "+", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(() => interpreter.Interpret(expr));
    }

    [TestMethod]
    public void Interpret_WhenNumberMinusBoolean_ReturnsRuntimeError()
    {
        // Arrange
        var left = new Literal(1.0);
        var right = new Literal(false);
        var op = new Token(TokenType.MINUS, "-", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(() => interpreter.Interpret(expr));
    }

    [TestMethod]
    public void Interpret_WhenTruePlusFalse_ReturnsRuntimeError()
    {
        // Arrange
        var left = new Literal(true);
        var right = new Literal(false);
        var op = new Token(TokenType.PLUS, "+", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(() => interpreter.Interpret(expr));
    }

    [TestMethod]
    public void Interpret_WhenStringMinusString_ReturnsRuntimeError()
    {
        // Arrange
        var left = new Literal("foo");
        var right = new Literal("bar");
        var op = new Token(TokenType.MINUS, "-", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(() => interpreter.Interpret(expr));
    }

    [TestMethod]
    public void Interpret_WhenComplexEqual_ReturnsTrue()
    {
        // Arrange 165 == (86 + 79)
        var left = new Literal(165.0);
        var right = new Binary(new Literal(86.0), new Token(TokenType.PLUS, "+", null, 1), new Literal(79.0)); // 86 + 79
        var op = new Token(TokenType.EQUAL_EQUAL, "==", null, 1);
        var expr = new Binary(left, op, right);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("true", result);
    }

    [TestMethod]
    public void Interpret_WhenNestedExpressions_ReturnsResult()
    {
        // Arrange
        var innerLeft = new Literal(2.0);
        var innerRight = new Literal(3.0);
        var innerOp = new Token(TokenType.PLUS, "+", null, 1);
        var innerExpr = new Binary(innerLeft, innerOp, innerRight);

        var outerLeft = new Literal(5.0);
        var outerOp = new Token(TokenType.EQUAL_EQUAL, "==", null, 1);
        var expr = new Binary(outerLeft, outerOp, innerExpr);
        var interpreter = new Interpreter("context");

        // Act
        var result = interpreter.Interpret(expr);

        // Assert
        Assert.AreEqual("true", result);
    }

    [TestMethod]
    public void Interpret_WhenPrintStatement_ReturnsResult()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var expression = new Literal("Hello, World!");
        var printStatement = new Print(expression);
        var interpreter = new Interpreter("context");

        // Act
        interpreter.Interpret([printStatement]);

        // Assert
        Assert.AreEqual("Hello, World!", writer.ToString().Trim());
    }

    [TestMethod]
    public void Interpret_WhenWhereStatement_ReturnsResult()
    {
        // Arrange
        var condition = new Literal(true);
        var whereStatement = new Where(condition);
        var interpreter = new Interpreter("context");

        // Act
        interpreter.Interpret([whereStatement]);

        // Assert
        throw new NotImplementedException("Assertion for Where statement result is not implemented.");
    }

    [TestMethod]
    public void Interpret_WhenSelectStatement_ReturnsResult()
    {
        // Arrange
        var expressions = new[] {
            new Variable(new Token(TokenType.IDENTIFIER, "myVar", null, 1), null) ,
            new Variable(new Token(TokenType.IDENTIFIER, "myVar2", null, 1), null)
        };
        var selectStatement = new Select(expressions);
        var interpreter = new Interpreter("context");

        // Act
        interpreter.Interpret([selectStatement]);

        // Assert
        throw new NotImplementedException("Assertion for Select statement result is not implemented.");
    }

    [TestMethod]
    public void Interpret_WhenAsStatement_ReturnsResult()
    {
        // Arrange
        var name = new Token(TokenType.IDENTIFIER, "myAlias", null, 1);
        var asStatement = new As(name);
        var interpreter = new Interpreter("context");

        // Act
        interpreter.Interpret([asStatement]);

        // Assert
        throw new NotImplementedException("Assertion for As statement result is not implemented.");
    }

    [TestMethod]
    public void Interpret_WhenFunctionStatement_ReturnsResult()
    {
        // Arrange
        var functionName = new Token(TokenType.IDENTIFIER, "myFunction", null, 1);
        var parameters = new List<Expression> { new Literal("param"), new Literal(2.0) };
        var functionStatement = new Statements.Function(functionName, parameters);
        var interpreter = new Interpreter("context");

        // Act
        interpreter.Interpret([functionStatement]);

        // Assert
        throw new NotImplementedException("Assertion for Function statement result is not implemented.");
    }
}
