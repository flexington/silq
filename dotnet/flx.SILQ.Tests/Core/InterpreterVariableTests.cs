using System;
using System.Collections.Generic;
using System.Linq;
using flx.SILQ.Core;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;

namespace flx.SILQ.Tests.Core;

[TestClass]
public class InterpreterVariableTests
{
    [TestMethod]
    public void VisitVariable_WhenPropertyIsNotFound_ThrowsRuntimeError()
    {
        // Arrange
        var variable = new Variable(new Token(TokenType.IDENTIFIER, "test", null, 1), null);
        var interpreter = new Interpreter(new object());

        // Act
        Action act = () => interpreter.Visit(variable);
        // Assert
        Assert.ThrowsException<RuntimeError>(act);
    }

    [TestMethod]
    public void VisitVariable_WhenPropertyIsFound_ReturnsValue()
    {
        // Arrange
        var variable = new Variable(new Token(TokenType.IDENTIFIER, "StringProperty", null, 1), null);
        var interpreter = new Interpreter(new TestContext());

        // Act
        var result = interpreter.Visit(variable);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("TestValue", result.ToString());
    }

    [TestMethod]
    public void VisitVariable_WhenNestedPropertyIsFound_ReturnsValue()
    {
        // Arrange
        var variable = new Variable(new Token(TokenType.IDENTIFIER, "NestedProperty", null, 1), new Variable(new Token(TokenType.IDENTIFIER, "NumberProperty", null, 1), null));
        var interpreter = new Interpreter(new TestContext());

        // Act
        var result = interpreter.Visit(variable);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(4, result);
    }

    [TestMethod]
    public void VisitVariable_WhenListPropertyIsFound_ReturnsValues()
    {
        // Arrange
        var variable = new Variable(new Token(TokenType.IDENTIFIER, "Items", null, 1), null);
        var interpreter = new Interpreter(new TestContext());

        // Act
        var result = interpreter.Visit(variable);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(List<TestSubject>));
        var list = (List<TestSubject>)result;
        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("John", list[0].Name);
    }


    internal record TestContext
    {
        public string StringProperty { get; } = "TestValue";

        public NestedContext NestedProperty { get; } = new NestedContext();

        public List<TestSubject> Items { get; } = new List<TestSubject>
        {
            new TestSubject { Name = "John", Age = 30, City = "New York" },
            new TestSubject { Name = "Jane", Age = 25, City = "Los Angeles" },
            new TestSubject { Name = "Alice", Age = 28, City = "New York" }
        };
    }

    internal record NestedContext
    {
        public int NumberProperty { get; set; } = 4;
    }

    internal record TestSubject
    {
        public string Name { get; init; }
        public int Age { get; init; }
        public string City { get; init; }
    }
}