using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Statements;

namespace flx.SILQ.Core;

/// <summary>
/// The Interpreter class implements the IVisitor interface to handle various statement types.
/// It processes the statements in the SILQ language, including print, from, and where statements.
/// </summary>
public partial class Interpreter : IVisitor
{
    /// <summary>
    /// Implements the visitor method for the <see cref="Print"/> statement.
    /// Evaluates the expression and writes its string representation to the console.
    /// </summary>
    /// <param name="print">The print statement to execute.</param>
    public void Visit(Print print)
    {
        object value = Evaluate(print.Expression);
        Console.WriteLine(Stringify(value));
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="From"/> statement.
    /// </summary>
    /// <param name="from">The "From" statement to process.</param>
    public object Visit(From from)
    {
        var context = _environment.Get("context");
        var variable = ResolveVariable(from.Property, context);

        if (from.Statement is not null)
        {
            var environment = new Environment(_environment);
            environment.SetContext(variable);
            _environment = environment;
            var result = Execute(from.Statement);
            _environment = _environment.Enclosing;
            return result;
        }

        return variable;
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Where"/> statement.
    /// </summary>
    /// <param name="where">The "Where" statement to process.</param>
    public object Visit(Where where)
    {
        var context = _environment.Get("context");
        if (!IsList(context)) throw new RuntimeError("context", "Context is not a list.");

        var input = (IList)context;
        if (input.Count == 0) throw new RuntimeError("context", "List is empty.");

        var output = new List<object>();
        foreach (var item in input)
        {
            var environment = new Environment(_environment);
            environment.SetContext(item);
            _environment = environment;

            var result = Evaluate(where.Expression);
            if (result is not bool) throw new RuntimeError("where", "Expression must evaluate to a boolean.");

            if ((bool)result) output.Add(item);

            _environment = _environment.Enclosing;
        }

        return output;
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Select"/> statement.
    /// </summary>
    /// <param name="select">The "Select" statement to process.</param>
    public object Visit(Select select)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="As"/> statement.
    /// </summary>
    /// <param name="statement">The "As" statement to process.</param>
    public void Visit(As statement)
    {
        var context = _environment.Get("context");
        if (context == null) throw new RuntimeError("As", "Context is null.");

        _environment.DefineGlobal(statement.Name.Lexeme, context);
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Function"/> statement.
    /// </summary>
    /// <param name="statement">The "Function" statement to process.</param>
    public void Visit(Statements.Function statement)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Count"/> statement.
    /// </summary>
    /// <param name="statement">The "Count" statement to process.</param>
    public double Visit(Count statement)
    {
        object result = null;
        if (statement.Expression != null) result = Evaluate(statement.Expression);
        else if (statement.Statement != null) result = Execute(statement.Statement);

        if (result == null) throw new RuntimeError("Count", "Cannot count null.");

        if (result.GetType() == typeof(string)) return ((string)result).Length;
        else if (result is IList list) return list.Count;
        else throw new RuntimeError("Count", "Context must be a string or list.");
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="First"/> statement.
    /// </summary>
    /// <param name="statement">The "First" statement to process.</param>
    public object Visit(First statement)
    {
        object result = null;
        if (statement.Statement != null) result = Execute(statement.Statement);

        if (result.GetType() == typeof(string)) return ((string)result)[0];
        else if (result is IList list) return list[0];
        else throw new RuntimeError("first", "Context is not a string or a list.");
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Last"/> statement.
    /// </summary>
    /// <param name="statement">The "Last" statement to process.</param>
    public object Visit(Last statement)
    {
        object result = null;
        if (statement.Statement != null) result = Execute(statement.Statement);

        if (result.GetType() == typeof(string)) return ((string)result)[^1];
        else if (result is IList list) return list[^1];
        else throw new RuntimeError("context", "Context is not a string or a list.");
    }
}