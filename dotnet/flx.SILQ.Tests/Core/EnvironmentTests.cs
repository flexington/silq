using System;
using flx.SILQ.Errors;

namespace flx.SILQ.Tests.Core;

[TestClass]
public class EnvironmentTests
{
    [TestMethod]
    public void SetContext_WhenContextIsNull_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.SetContext(null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void SetContext_WhenContextIsNotNull_SetsContext()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();
        var context = new EnvironmentTestContext();

        // Act
        environment.SetContext(context);

        // Assert
        Assert.AreEqual(context, environment.Get("context"));
    }

    [TestMethod]
    public void Define_WhenNameIsNull_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Define(null, null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Define_WhenNameIsEmpty_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Define(string.Empty, null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Define_WhenNameIsWhitespace_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Define("   ", null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Define_WhenNameIsContext_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Define("context", null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Define_WhenNameIsAlreadyDefined_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();
        var name = "testVar";
        environment.Define(name, 42);

        // Act
        Action action = () => environment.Define(name, null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Get_WhenNameIsNull_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Get(null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Get_WhenNameIsEmpty_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Get(string.Empty);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Get_WhenNameIsWhitespace_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Get("   ");

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Get_WhenNameIsUndefined_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();
        var name = "testVar";

        // Act
        Action action = () => environment.Get(name);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Get_WhenNameIsDefined_ReturnsValue()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();
        var name = "testVar";
        var value = 42;
        environment.Define(name, value);

        // Act
        var result = environment.Get(name);

        // Assert
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void Get_WhenNameIsDefinedInParentContext_ReturnsValue()
    {
        // Arrange
        var parentEnvironment = new SILQ.Core.Environment();
        var childEnvironment = new SILQ.Core.Environment(parentEnvironment);
        var name = "testVar";
        var value = 42;
        parentEnvironment.Define(name, value);

        // Act
        var result = childEnvironment.Get(name);

        // Assert
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void Set_WhenNameIsNull_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Set(null, null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Set_WhenNameIsEmpty_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Set(string.Empty, null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Set_WhenNameIsWhitespace_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();

        // Act
        Action action = () => environment.Set("   ", null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Set_WhenNameIsUndefined_ThrowsRuntimeError()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();
        var name = "testVar";

        // Act
        Action action = () => environment.Set(name, null);

        // Act & Assert
        Assert.ThrowsException<RuntimeError>(action);
    }

    [TestMethod]
    public void Set_WhenNameIsDefinedInParentContext_SetsValue()
    {
        // Arrange
        var parentEnvironment = new SILQ.Core.Environment();
        var childEnvironment = new SILQ.Core.Environment(parentEnvironment);
        var name = "testVar";
        var value = 42;
        parentEnvironment.Define(name, value);

        // Act
        childEnvironment.Set(name, 100);

        // Assert
        Assert.AreEqual(100, parentEnvironment.Get(name));
    }

    [TestMethod]
    public void Set_WhenNameIsDefinedInCurrentContext_SetsValue()
    {
        // Arrange
        var environment = new SILQ.Core.Environment();
        var name = "testVar";
        var value = 42;
        environment.Define(name, value);

        // Act
        environment.Set(name, 100);

        // Assert
        Assert.AreEqual(100, environment.Get(name));
    }
}

internal class EnvironmentTestContext
{

}