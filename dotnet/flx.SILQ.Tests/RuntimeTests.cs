using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using flx.SILQ.Errors;
using flx.SILQ.Models;

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
    public void Execute_WhenFromWhere_ReturnList()
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
    public void Execute_WhenFromWhereAnd_ReturnsList()
    {
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
    public void Execute_WhenFromWhereOr_ReturnsList()
    {
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

    [TestMethod]
    public void Execute_WhenCountFromString_ReturnsLength()
    {
        // Arrange
        var query = "count from School.Name;";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(double));
        Assert.AreEqual(11.0, result);
    }

    [TestMethod]
    public void Execute_WhenCountFromList_ReturnsCount()
    {
        // Arrange
        var query = "count from School.Students;";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(double));
        Assert.AreEqual(3.0, result);
    }

    [TestMethod]
    public void Execute_WhenCountFromNumber_ThrowsRuntimeError()
    {
        // Arrange
        var query = "count from School.NumberOfStudents;";

        // Act
        var runtime = new Runtime(_testContext);
        Action action = () => runtime.Execute(query);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Execute_WhenFirstFromList_ReturnsFirstElement()
    {
        // Arrange
        var query = "first from School.Students;";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(Student));
        Assert.AreEqual("Alice", ((Student)result).Name);
    }

    [TestMethod]
    public void Execute_WhenFirstFromString_ReturnsFirstCharacter()
    {
        // Arrange
        var query = "first from School.Name;";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(char));
        Assert.AreEqual('T', (char)result);
    }

    [TestMethod]
    public void Execute_WhenFirstFromNumber_ThrowsRuntimeError()
    {
        // Arrange
        var query = "first from School.NumberOfStudents;";

        // Act
        var runtime = new Runtime(_testContext);
        Action action = () => runtime.Execute(query);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Execute_WhenLastFromList_ReturnsLastElement()
    {
        // Arrange
        var query = "last from School.Students;";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(Student));
        Assert.AreEqual("Charlie", ((Student)result).Name);
    }

    [TestMethod]
    public void Execute_WhenLastFromString_ReturnsLastCharacter()
    {
        // Arrange
        var query = "last from School.Name;";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsInstanceOfType(result, typeof(char));
        Assert.AreEqual('l', (char)result);
    }

    [TestMethod]
    public void Execute_WhenLastFromNumber_ThrowsRuntimeError()
    {
        // Arrange
        var query = "last from School.NumberOfStudents;";

        // Act
        var runtime = new Runtime(_testContext);
        Action action = () => runtime.Execute(query);

        // Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Execute_WhenFromAs_ReturnsNull()
    {
        // Arrange
        var query = "from School.Students as Student;";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Execute_WhenFromSelectListSingle_ReturnsList()
    {
        // Arrange
        var query = "from School.Students select { Name };";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList));

        var list = (List<object>)result;
        Assert.AreEqual(3, list.Count);
        Assert.IsInstanceOfType(list[0], typeof(SelectObject));

        dynamic so = (SelectObject)list[0];
        Assert.AreEqual("Alice", so.Name);
    }

    [TestMethod]
    public void Execute_WhenFromSelectListMultiple_ReturnsList()
    {
        // Arrange
        var query = "from School.Students select { Name, Age };";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList));

        var list = (List<object>)result;
        Assert.AreEqual(3, list.Count);
        Assert.IsInstanceOfType(list[0], typeof(SelectObject));

        dynamic so = (SelectObject)list[0];
        Assert.AreEqual("Alice", so.Name);
        Assert.AreEqual(14, so.Age);
    }

    [TestMethod]
    public void Execute_WhenFromSelectObjectSingle_ReturnObject()
    {
        // Arrange
        var query = "from School select { Name };";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(SelectObject));

        dynamic so = (SelectObject)result;
        Assert.AreEqual("Test School", so.Name);
    }

    [TestMethod]
    public void Execute_WhenFromSelectObjectMultiple_ReturnObject()
    {
        // Arrange
        var query = "from School select { Name, Address };";

        // Act
        var runtime = new Runtime(_testContext);
        var result = runtime.Execute(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(SelectObject));

        dynamic so = (SelectObject)result;
        Assert.AreEqual("Test School", so.Name);
        Assert.AreEqual("123 Test St", so.Address);
    }
}