using System.Collections.Generic;
using System.Dynamic;
using flx.SILQ.Errors;

namespace flx.SILQ.Models;

/// <summary>
/// Represents a dynamic object used for SILQ select projections.
/// <para>
/// This class allows dynamic creation and access of properties at runtime, enabling flexible
/// projection of fields in SILQ queries (e.g., <c>select { Name, Age }</c>).
/// </para>
/// </summary>
public sealed class SelectObject : DynamicObject
{
    private readonly Dictionary<string, object> _properties = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectObject"/> class with the specified properties.
    /// </summary>
    /// <param name="properties">A dictionary of property names and values to initialize the object with.</param>
    public SelectObject(Dictionary<string, object> properties)
    {
        _properties = properties;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectObject"/> class with a single property.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    public SelectObject(string name, object value)
    {
        _properties.Add(name, value);
    }

    /// <summary>
    /// Gets the value of a property by name, with validation and error handling.
    /// </summary>
    /// <param name="name">The name of the property to retrieve. Must not be null, empty, whitespace, or contain a dot ('.').</param>
    /// <returns>The value of the property if found.</returns>
    /// <exception cref="RuntimeError">Thrown if the name is invalid or the property does not exist.</exception>
    public object GetMemeber(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name)) throw new RuntimeError("name", "Name cannot be null, empty, or whitespace");
        if (name.Contains('.')) throw new RuntimeError("name", "Name cannot contain '.'");
        if (!_properties.ContainsKey(name)) throw new RuntimeError("name", $"Property '{name}' not found");
        return _properties[name];
    }

    /// <summary>
    /// Attempts to get the value of a dynamic property.
    /// </summary>
    /// <param name="binder">Provides information about the member to get.</param>
    /// <param name="result">The value of the property, if found.</param>
    /// <returns>True if the property exists; otherwise, false.</returns>
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        if (_properties.TryGetValue(binder.Name, out result))
        {
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Attempts to set the value of a dynamic property.
    /// </summary>
    /// <param name="binder">Provides information about the member to set.</param>
    /// <param name="value">The value to set for the property.</param>
    /// <returns>Always returns true.</returns>
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        _properties[binder.Name] = value;
        return true;
    }
}