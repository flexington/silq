using System.Collections.Generic;
using flx.SILQ.Expressions;
using flx.SILQ.Models;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a function statement in the SILQ language.
/// </summary>
/// <remarks>
/// A function statement consists of a name and a list of arguments. It inherits from the base <see cref="Statement"/> class.
/// </remarks>
public record Function(Token Name, List<Expression> Arguments) : Statement
{
    /// <summary>
    /// Accepts a visitor that implements the <see cref="IVisitor"/> interface.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    public override object Accept(IVisitor visitor)
    {
        visitor.Visit(this);
        return null;
    }
}