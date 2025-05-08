using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a "from" statement in the SILQ language.
/// This statement is used to specify the source of data for a query.
/// </summary>
public record From(Expression Expression) : Statement
{
    /// <summary>
    /// Accepts a visitor to process this from statement.
    /// </summary>
    /// <param name="visitor">The visitor that will process the from statement.</param>
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}