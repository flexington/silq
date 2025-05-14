using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using flx.SILQ.Core;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Statements;

namespace flx.SILQ.Tests.Core;

[TestClass]
public class InterpreterModifierTests
{
    [TestMethod]
    public void Count_WhenContextIsString_ReturnsCount()
    {
        // Arrange
        var count = new Count(new Literal("TestContext"));
        var interpreter = new Interpreter("TestContext");

        // Act
        var result = interpreter.Visit(count);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(double));
        Assert.AreEqual(11, result);
    }

    [TestMethod]
    public void Count_WhenContextIsList_ReturnsCount()
    {
        // Arrange
        var count = new Count(new Literal(new List<string> { "Item1", "Item2", "Item3" }));
        var interpreter = new Interpreter(new List<string> { "Item1", "Item2", "Item3" });

        // Act
        var result = interpreter.Visit(count);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(double));
        Assert.AreEqual(3, result);
    }

    [TestMethod]
    public void Count_WhenContextIsNumber_ThrowsRuntimeError()
    {
        // Arrange
        var count = new Count(new Literal(42.0));
        var interpreter = new Interpreter(42.0);

        // Act
        Action act = () => interpreter.Visit(count);

        // Assert
        Assert.ThrowsException<RuntimeError>(act);
    }

    [TestMethod]
    public void First_WhenContextIsString_ReturnsFirstCharacter()
    {
        // Arrange
        var first = new First();
        var interpreter = new Interpreter("TestContext");

        // Act
        interpreter.Visit(first);
        var result = interpreter.GetContext();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(char));
        Assert.AreEqual('T', result);
    }

    [TestMethod]
    public void First_WhenContextIsList_ReturnsFirstItem()
    {
        // Arrange
        var first = new First();
        var interpreter = new Interpreter(new List<string> { "Item1", "Item2", "Item3" });

        // Act
        interpreter.Visit(first);
        var result = interpreter.GetContext();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(string));
        Assert.AreEqual("Item1", result);
    }

    [TestMethod]
    public void First_WhenContextIsNumber_ThrowsRuntimeError()
    {
        // Arrange
        var first = new First();
        var interpreter = new Interpreter(42.0);

        // Act
        Action act = () => interpreter.Visit(first);

        // Assert
        Assert.ThrowsException<RuntimeError>(act);
    }

    [TestMethod]
    public void Last_WhenContextIsString_ReturnsLastCharacter()
    {
        // Arrange
        var last = new Last();
        var interpreter = new Interpreter("TestContext");

        // Act
        interpreter.Visit(last);
        var result = interpreter.GetContext();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(char));
        Assert.AreEqual('t', result);
    }

    [TestMethod]
    public void Last_WhenContextIsList_ReturnsLastItem()
    {
        // Arrange
        var last = new Last();
        var interpreter = new Interpreter(new List<string> { "Item1", "Item2", "Item3" });

        // Act
        interpreter.Visit(last);
        var result = interpreter.GetContext();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(string));
        Assert.AreEqual("Item3", result);
    }

    [TestMethod]
    public void Last_WhenContextIsNumber_ThrowsRuntimeError()
    {
        // Arrange
        var last = new Last();
        var interpreter = new Interpreter(42.0);

        // Act
        Action act = () => interpreter.Visit(last);

        // Assert
        Assert.ThrowsException<RuntimeError>(act);
    }
}