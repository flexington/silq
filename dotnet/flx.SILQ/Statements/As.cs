using flx.SILQ.Models;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents an 'As' statement in the SILQ language.
/// </summary>
/// <remarks>
/// This statement is used to define a specific behavior or operation in the SILQ language.
/// It inherits from the base <see cref="Statement"/> class and implements the <see cref="IVisitor"/> pattern.
/// </remarks>
public record As(Token Name) : Statement
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