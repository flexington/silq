using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using flx.SILQ.Errors;

namespace flx.SILQ.Tests;

[TestClass]
public class RuntimeTests
{
    TestContext _testContext;

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
                    new Student { Name = "Alice", Age = 14, Grade = "9th" },
                    new Student { Name = "Bob", Age = 15, Grade = "10th" },
                    new Student { Name = "Charlie", Age = 14, Grade = "9th" }
                }
            }
        };
    }

    [TestMethod]
    public void Runtime_WhenContextIsNull_ThrowsRuntimeError()
    {
        // Arrange
        object context = null;

        // Act
        Action action = () => new Runtime(context);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Execute_WhenQueryIsNull_ThrowsRuntimeError()
    {
        // Arrange
        var context = new object();
        string query = null;

        // Act
        var runtime = new Runtime(context);
        Action action = () => runtime.Execute(query);


        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Execute_WhenQueryIsEmpty_ThrowsRuntimeError()
    {
        // Arrange
        var context = new object();
        string query = string.Empty;

        // Act
        var runtime = new Runtime(context);
        Action action = () => runtime.Execute(query);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Execute_WhenQueryIsWhitespace_ThrowsRuntimeError()
    {
        // Arrange
        var context = new object();
        string query = "   ";

        // Act
        var runtime = new Runtime(context);
        Action action = () => runtime.Execute(query);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Execute_WhenPrint_OutputExpression()
    {
        // Arrange
        var context = new object();
        var query = "print \"Hello, World!\";";

        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var runtime = new Runtime(context);
        runtime.Execute(query);

        // Assert
        var result = output.ToString().Trim();
        Assert.AreEqual("Hello, World!", result);
    }

    [TestMethod]
    public void Execute_WhenWhere_ReturnList()
    {
        // Arrange
        var query = "from School.Students where Age == 14";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(IList));
        var students = (List<object>)result;
        Assert.AreEqual(2, students.Count);
        Assert.IsInstanceOfType(students[0], typeof(Student));
        Assert.IsInstanceOfType(students[1], typeof(Student));
    }

    [TestMethod]
    public void Execute_WhenWhereAnd_ReturnsList(){
        // Arrange
        var query = "from School.Students where Age == 14 and Grade == \"9th\";";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(IList));
        var students = (List<object>)result;
        Assert.AreEqual(2, students.Count);
    }

    [TestMethod]
    public void Execute_WhenWhereOr_ReturnsList(){
        // Arrange
        var query = "from School.Students where Name == \"Alice\" or Grade == \"10th\";";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(IList));
        var students = (List<object>)result;
        Assert.AreEqual(2, students.Count);
    }



    internal record TestContext
    {
        public School School { get; set; }
    }

    internal record School
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Student> Students { get; set; }
    }

    internal record Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Grade { get; set; }
        public School School { get; set; }
    }
}