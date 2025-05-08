using System;
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
        throw new NotImplementedException();
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
}