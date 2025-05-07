using System;

namespace flx.SILQ.Statements;

/// <summary>
/// Implements the <see cref="IVisitor"/> interface for statement nodes in the SILQ AST.
/// Provides visit methods for supported statement types.
/// </summary>
public class Visitor : IVisitor
{
    /// <summary>
    /// Visits a generic <see cref="Statement"/> node.
    /// Throws <see cref="NotImplementedException"/> by default.
    /// </summary>
    /// <param name="statement">The statement node to visit.</param>
    public void Visit(Statement statement)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Visits a <see cref="Print"/> statement node.
    /// Throws <see cref="NotImplementedException"/> by default.
    /// </summary>
    /// <param name="print">The print statement node to visit.</param>
    public void Visit(Print print)
    {
        throw new NotImplementedException();
    }
}