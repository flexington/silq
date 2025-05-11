using System;
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
        if (context == null) throw new RuntimeError(context, "Context is not set.");
        return Visit(from.Property, context);
    }

    private object Visit(Variable property, object context)
    {
        if (context == null) throw new RuntimeError(property.Name, "Context is not set.");

        var prop = context.GetType().GetProperty(property.Name.Lexeme);
        if (prop == null) throw new RuntimeError(property.Name, $"The identifier '{property.Name.Lexeme}' is not defined in the current context.");

        if (property.Member is not null)
        {
            return Visit(property.Member, prop.GetValue(context));
        }

        return prop.GetValue(context);
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Where"/> statement.
    /// </summary>
    /// <param name="where">The "Where" statement to process.</param>
    public void Visit(Where where)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Implements the visitor method for the <see cref="Select"/> statement.
    /// </summary>
    /// <param name="select">The "Select" statement to process.</param>
    public void Visit(Select select)
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