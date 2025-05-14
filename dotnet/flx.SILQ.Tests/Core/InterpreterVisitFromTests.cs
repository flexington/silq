using System;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;
using flx.SILQ.Statements;

namespace flx.SILQ.Core.Tests;

[TestClass]
public class InterpreterVisitFromTests
{
    [TestMethod]
    public void VisitFrom_WhenPropertyIsNotFound_ThrowsRuntimeError()
    {
        // Arrange
        var fromStatement = new From(new Variable(new Token(TokenType.IDENTIFIER, "test", null, 1), null));
        var interpreter = new Interpreter(new object());

        // Act
        Action act = () => interpreter.Visit(fromStatement);
        // Assert
        Assert.ThrowsException<RuntimeError>(act);
    }

    [TestMethod]
    public void VisitFrom_WhenPropertyIsFound_RetrunsPropertyValue()
    {
        // Arrange
        var fromStatement = new From(new Variable(new Token(TokenType.IDENTIFIER, "Property", null, 1), null));
        var interpreter = new Interpreter(new TestContext());

        // Act
        interpreter.Visit(fromStatement);
        var result = interpreter.GetContext();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(string));
        Assert.AreEqual("TestValue", result);
    }

    [TestMethod]
    public void VisitFrom_WhenNestedPropertyIsFound_ReturnsNestedPropertyValue()
    {
        // Arrange
        var nestedProperty = new Variable(new Token(TokenType.IDENTIFIER, "Property", null, 1), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "Nested", null, 1), nestedProperty);
        var fromStatement = new From(property);
        var interpreter = new Interpreter(new TestContext());

        // Act
        interpreter.Visit(fromStatement);
        var result = interpreter.GetContext();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(string));
        Assert.AreEqual("NestedValue", result);
    }

}

internal class TestContext
{
    public string Property { get; set; } = "TestValue";

    public NestedContext Nested { get; set; } = new NestedContext();

    internal class NestedContext
    {
        public string Property { get; set; } = "NestedValue";
    }
}