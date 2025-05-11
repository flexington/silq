using System.Collections.Generic;
using flx.SILQ.Expressions;
using flx.SILQ.Models;
using flx.SILQ.Statements;

namespace flx.SILQ.Core.Tests;

[TestClass]
public class InterpreterWhereTests
{
    [TestMethod]
    // where a = 1
    public void VisitWhere()
    {
        // Arrange
        var left = new Variable(new Token(TokenType.IDENTIFIER, "a", "a", 1));
        var right = new Literal(1);
        var op = new Token(TokenType.EQUAL_EQUAL, "==", "==", 1);
        var expression = new Binary(left, op, right);
        var where = new Where(expression);
        var interpreter = new Interpreter(new TestContext().Items);

        // Act
        interpreter.Visit(where);
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