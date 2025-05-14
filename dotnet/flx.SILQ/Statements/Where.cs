using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a "Where" statement in the SILQ language.
/// </summary>
/// <remarks>
/// This statement is used to filter elements based on a condition.
/// </remarks>
/// <param name="Expression">The condition or set of conditions to filter elements.</param>
public record Where(Expression Expression, Statement Statement) : Statement
{
    /// <summary>
    /// Accepts a visitor to process this "Where" statement.
    /// </summary>
    /// <param name="visitor">The visitor that processes this statement.</param>
    public override object Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}