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
    private readonly Environment _enclosing;

    public Environment(Environment enclosing = null)
    {
        _enclosing = enclosing;
    }

    public void Define(string name, object value)
    {
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
        if (_variables.TryGetValue(name, out var value)) return value;
        if (_enclosing != null) return _enclosing.Get(name);

        throw new RuntimeError(name, $"Undefined variable '{name}'.");
    }

    /// <summary>
    /// Sets the value of an existing variable in the environment.
    /// </summary>
    /// <param name="name">The name of the variable to set.</param>
    /// <param name="value">The new value to assign to the variable.</param>
    public void Set(string name, object value)
    {
        if (_variables.ContainsKey(name))
        {
            _variables[name] = value;
            return;
        }

        if (_enclosing != null)
        {
            _enclosing.Set(name, value);
            return;
        }

        throw new RuntimeError(name, $"Undefined variable '{name}'.");
    }
}

