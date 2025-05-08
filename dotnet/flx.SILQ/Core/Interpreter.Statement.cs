using System;
using flx.SILQ.Statements;

namespace flx.SILQ.Core;

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

    public void Visit(From from)
    {
        throw new NotImplementedException();
    }
}