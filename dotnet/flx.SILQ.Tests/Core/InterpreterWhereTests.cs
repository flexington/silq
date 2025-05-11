using System;
using System.Collections;
using System.Collections.Generic;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;
using flx.SILQ.Statements;

namespace flx.SILQ.Core.Tests;

[TestClass]
public class InterpreterWhereTests
{
    [TestMethod]
    public void VisitWhere_WhenContextIsNotList_ThrowsRuntimeError()
    {
        // Arrange
        var context = new object();
        var interpreter = new Interpreter(context);
        var expression = new Literal(1);
        var where = new Where(expression);

        // Act
        Action action = () => interpreter.Visit(where);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void VisitWhere_WhenContextIsEmptyList_ThrowsRuntimeError()
    {
        // Arrange
        var context = new List<TestSubject>();
        var interpreter = new Interpreter(context);
        var expression = new Literal(1);
        var where = new Where(expression);

        // Act
        Action action = () => interpreter.Visit(where);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void VisitWhere_WhenExpressionIsNotBoolean_ThrowsRuntimeError()
    {
        // Arrange
        var interpreter = new Interpreter(new TestContext().Items);
        var expression = new Literal(1);
        var where = new Where(expression);

        // Act
        Action action = () => interpreter.Visit(where);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void VisitWhere_WhenGenericTrue_ReturnsAllListItems()
    {
        // Arrange
        var left = new Literal(1);
        var right = new Literal(1);
        var op = new Token(TokenType.EQUAL_EQUAL, "==", "==", 1);
        var expression = new Binary(left, op, right);
        var where = new Where(expression);
        var interpreter = new Interpreter(new TestContext().Items);

        // Act
        var result = interpreter.Visit(where);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList));
        var list = (IList)result;
        Assert.AreEqual(15, list.Count);
    }

    [TestMethod]
    public void VisitWhere_WhenGenericFalse_ReturnsNoListItems()
    {
        // Arrange
        var left = new Literal(1);
        var right = new Literal(2);
        var op = new Token(TokenType.EQUAL_EQUAL, "==", "==", 1);
        var expression = new Binary(left, op, right);
        var where = new Where(expression);
        var interpreter = new Interpreter(new TestContext().Items);

        // Act
        var result = interpreter.Visit(where);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList));
        var list = (IList)result;
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    public void VisitWhere_WhenPropertyIsTrue_ReturnsMatchingListItems()
    {
        // Arrange
        var left = new Variable(new Token(TokenType.IDENTIFIER, "Name", "Name", 1));
        var op = new Token(TokenType.EQUAL_EQUAL, "==", "==", 1);
        var right = new Literal("John");
        var expression = new Binary(left, op, right);
        var where = new Where(expression);
        var interpreter = new Interpreter(new TestContext().Items);

        // Act
        var result = interpreter.Visit(where);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList));
        var list = (IList)result;
        Assert.AreEqual(2, list.Count);
    }

    [TestMethod]
    public void VisitWhere_WhenPropertyIsFalse_ReturnsNoListItems()
    {
        // Arrange
        var left = new Variable(new Token(TokenType.IDENTIFIER, "Name", "Name", 1));
        var op = new Token(TokenType.EQUAL_EQUAL, "==", "==", 1);
        var right = new Literal("Martha");
        var expression = new Binary(left, op, right);
        var where = new Where(expression);
        var interpreter = new Interpreter(new TestContext().Items);

        // Act
        var result = interpreter.Visit(where);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList));
        var list = (IList)result;
        Assert.AreEqual(0, list.Count);
    }

    internal record TestContext()
    {
        public List<TestSubject> Items { get; } = new List<TestSubject>{
            new TestSubject { Name = "John", Age = 30, City = "New York" },
            new TestSubject { Name = "Jane", Age = 25, City = "Los Angeles" },
            new TestSubject { Name = "John", Age = 30, City = "Chicago" },
            new TestSubject { Name = "Alice", Age = 28, City = "New York" },
            new TestSubject { Name = "Bob", Age = 35, City = "Los Angeles" },
            new TestSubject { Name = "Charlie", Age = 22, City = "Chicago" },
            new TestSubject { Name = "David", Age = 30, City = "New York" },
            new TestSubject { Name = "Eve", Age = 25, City = "Los Angeles" },
            new TestSubject { Name = "Frank", Age = 28, City = "Chicago" },
            new TestSubject { Name = "Grace", Age = 35, City = "New York" },
            new TestSubject { Name = "Heidi", Age = 22, City = "Los Angeles" },
            new TestSubject { Name = "Ivan", Age = 30, City = "Chicago" },
            new TestSubject { Name = "Judy", Age = 25, City = "New York" },
            new TestSubject { Name = "Mallory", Age = 28, City = "Los Angeles" },
            new TestSubject { Name = "Niaj", Age = 35, City = "Chicago" }
        };
    }

    internal class TestSubject
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }
}