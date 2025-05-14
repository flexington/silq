using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a print statement in the SILQ language.
/// When executed, evaluates the contained expression and outputs its value.
/// </summary>
/// <param name="Expression">The expression to be printed.</param>
public record Print(Expression Expression) : Statement
{
    /// <summary>
    /// Accepts a visitor to process this print statement.
    /// </summary>
    /// <param name="visitor">The visitor that will process the print statement.</param>
    public override object Accept(IVisitor visitor)
    {
        visitor.Visit(this);
        return null;
    }
}