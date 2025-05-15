using System;
using System.Collections.Generic;
using flx.SILQ.Core;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;
using flx.SILQ.Statements;

namespace flx.SILQ.Tests.Core;

[TestClass]
public class InterpreterModifierTests
{
    private TestContext _testContext;

    [TestInitialize]
    public void Initialize()
    {
        _testContext = new TestContext
        {
            School = new School
            {
                Name = "Test School",
                Address = "123 Test St",
                Students = new List<Student>
                {
                    new Student { Name = "John Doe", Age = 16, Grade = "10th" },
                    new Student { Name = "Jane Smith", Age = 17, Grade = "11th" }
                }
            }
        };
    }

    [TestMethod]
    public void Count_WhenContextIsString_ReturnsCount()
    {
        // Arrange
        var member = new Variable(new Token(TokenType.IDENTIFIER, "Name", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var count = new Count(new From(property, null));
        var interpreter = new Interpreter(_testContext);

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
        var member = new Variable(new Token(TokenType.IDENTIFIER, "Students", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var count = new Count(new From(property, null));
        var interpreter = new Interpreter(_testContext);

        // Act
        var result = interpreter.Visit(count);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(double));
        Assert.AreEqual(2, result);
    }

    [TestMethod]
    public void Count_WhenContextIsNumber_ThrowsRuntimeError()
    {
        // Arrange
        var member = new Variable(new Token(TokenType.IDENTIFIER, "NumberOfStudents", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var count = new Count(new From(property, null));
        var interpreter = new Interpreter(_testContext);

        // Act
        Action act = () => interpreter.Visit(count);

        // Assert
        Assert.ThrowsException<RuntimeError>(act);
    }

    [TestMethod]
    public void First_WhenContextIsString_ReturnsFirstCharacter()
    {
        // Arrange
        var member = new Variable(new Token(TokenType.IDENTIFIER, "Name", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var first = new First(new From(property, null));
        var interpreter = new Interpreter(_testContext);

        // Act
        var result = interpreter.Visit(first);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(char));
        Assert.AreEqual('T', result);
    }

    [TestMethod]
    public void First_WhenContextIsList_ReturnsFirstItem()
    {
        // Arrange
        var member = new Variable(new Token(TokenType.IDENTIFIER, "Students", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var first = new First(new From(property, null));
        var interpreter = new Interpreter(_testContext);

        // Act
        var result = interpreter.Visit(first);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(Student));
        Assert.AreEqual("John Doe", ((Student)result).Name);
    }

    [TestMethod]
    public void First_WhenContextIsNumber_ThrowsRuntimeError()
    {
        // Arrange
        var member = new Variable(new Token(TokenType.IDENTIFIER, "NumberOfStudents", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var first = new First(new From(property, null));
        var interpreter = new Interpreter(_testContext);

        // Act
        Action act = () => interpreter.Visit(first);

        // Assert
        Assert.ThrowsException<RuntimeError>(act);
    }

    [TestMethod]
    public void Last_WhenContextIsString_ReturnsLastCharacter()
    {
        // Arrange
        var member = new Variable(new Token(TokenType.IDENTIFIER, "Name", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var last = new Last(new From(property, null));
        var interpreter = new Interpreter(_testContext);

        // Act
        var result = interpreter.Visit(last);


        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(char));
        Assert.AreEqual('l', result);
    }

    [TestMethod]
    public void Last_WhenContextIsList_ReturnsLastItem()
    {
        // Arrange
        var member = new Variable(new Token(TokenType.IDENTIFIER, "Students", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var last = new Last(new From(property, null));
        var interpreter = new Interpreter(_testContext);

        // Act
        var result = interpreter.Visit(last);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(Student));
        Assert.AreEqual("Jane Smith", ((Student)result).Name);
    }

    [TestMethod]
    public void Last_WhenContextIsNumber_ThrowsRuntimeError()
    {
        // Arrange
        var member = new Variable(new Token(TokenType.IDENTIFIER, "NumberOfStudents", null, 0), null);
        var property = new Variable(new Token(TokenType.IDENTIFIER, "School", null, 0), member);
        var last = new Last(new From(property, null));
        var interpreter = new Interpreter(_testContext);

        // Act
        Action act = () => interpreter.Visit(last);

        // Assert
        Assert.ThrowsException<RuntimeError>(act);
    }
}