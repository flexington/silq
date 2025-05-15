using System;
using System.Collections.Generic;
using flx.SILQ.Errors;

namespace flx.SILQ.Core;

/// <summary>
/// Represents an environment for storing variables and their values in the SILQ interpreter.
/// </summary>
/// <remarks>
/// The environment is used to manage variable definitions and lookups during the execution of SILQ programs.
/// </remarks>
public class Environment
{
    /// <summary>
    /// Gets the dictionary of variables and their associated values.
    /// </summary>
    private readonly Dictionary<string, object> _variables = new();

    /// <summary>
    /// The enclosing environment for nested scopes.
    /// </summary>
    public Environment Enclosing { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Environment"/> class.
    /// </summary>
    /// <param name="enclosing">The enclosing environment for nested scopes.</param>
    public Environment(Environment enclosing = null)
    {
        Enclosing = enclosing;
    }

    /// <summary>
    /// Sets the context for the environment.
    /// </summary>
    /// <param name="context">The context object to set.</param>
    /// <exception cref="RuntimeError">Thrown if the context is null.</exception>
    public void SetContext(object context)
    {

        if (context == null) throw new RuntimeError("context", "Context cannot be null.");
        _variables["context"] = context;
    }

    /// <summary>
    /// Defines a new variable in the environment.
    /// </summary>
    /// <param name="name">The name of the variable to define.</param>
    /// <param name="value">The value to assign to the variable.</param>
    /// <exception cref="RuntimeError">Thrown if the variable name is null or empty.</exception>
    /// <exception cref="RuntimeError">Thrown if the variable name is 'context'.</exception>
    public void Define(string name, object value)
    {
        if (string.IsNullOrEmpty(name)) throw new RuntimeError(name, "Variable name cannot be null or empty.");
        if (string.IsNullOrWhiteSpace(name)) throw new RuntimeError(name, "Variable name cannot be empty or whitespace.");
        if (name == "context") throw new RuntimeError(name, "The identifier 'context' is reserved and cannot be used.");
        if (_variables.ContainsKey(name)) throw new RuntimeError(name, $"The identifier '{name}' is already defined in the current context.");

        _variables[name] = value;
    }

    /// <summary>
    /// Retrieves the value of a variable from the environment.
    /// </summary>
    /// <param name="name">The name of the variable to retrieve.</param>
    /// <returns>The value of the variable.</returns>
    /// <exception cref="RuntimeError">Thrown if the variable is not defined in the environment.</exception>
    public object Get(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new RuntimeError(name, "Variable name cannot be null or empty.");
        if (_variables.TryGetValue(name, out var value)) return value;
        if (Enclosing != null) return Enclosing.Get(name);

        throw new RuntimeError(name, $"Undefined variable '{name}'.");
    }

    /// <summary>
    /// Sets the value of an existing variable in the environment.
    /// </summary>
    /// <param name="name">The name of the variable to set.</param>
    /// <param name="value">The new value to assign to the variable.</param>
    /// <exception cref="RuntimeError">Thrown if the variable is not defined in the environment.</exception>
    /// <exception cref="RuntimeError">Thrown if the variable name is null or empty.</exception>
    public void Set(string name, object value)
    {
        if (string.IsNullOrEmpty(name)) throw new RuntimeError(name, "Variable name cannot be null or empty.");

        if (_variables.ContainsKey(name))
        {
            _variables[name] = value;
            return;
        }

        if (Enclosing != null)
        {
            Enclosing.Set(name, value);
            return;
        }

        throw new RuntimeError(name, $"Undefined variable '{name}'.");
    }

    /// <summary>
    /// Defines a global variable in the environment.
    /// </summary>
    /// <param name="name">The name of the variable to define.</param>
    /// <param name="value">The value to assign to the variable.</param>
    /// <exception cref="RuntimeError">Thrown if the variable name is null, empty or whitespace.</exception>
    /// <exception cref="RuntimeError">Thrown if the value is null.</exception>
    public void DefineGlobal(string name, object value)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new RuntimeError(name, "Variable name cannot be null, empty, or whitespace.");
        if (value == null) throw new RuntimeError(name, "Value cannot be null.");

        if (Enclosing != null)
        {
            Enclosing.DefineGlobal(name, value);
            return;
        }

        Define(name, value);
    }
}

