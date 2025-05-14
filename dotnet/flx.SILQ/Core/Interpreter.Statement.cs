using System;
using System.Collections;
using System.Collections.Generic;
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
    public void Visit(From from)
    {
        var context = _environment.Get("context");
        var result = ResolveVariable(from.Property, context);
        var environment = new Environment(_environment);
        environment.SetContext(result);
        _environment = environment;
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Where"/> statement.
    /// </summary>
    /// <param name="where">The "Where" statement to process.</param>
    public void Visit(Where where)
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

        _environment.SetContext(output);
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
        throw new NotImplementedException();
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Function"/> statement.
    /// </summary>
    /// <param name="statement">The "Function" statement to process.</param>
    public void Visit(Statements.Function statement)
    {
        throw new NotImplementedException();
    }
}