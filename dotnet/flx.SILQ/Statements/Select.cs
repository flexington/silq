using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a "Select" statement in the SILQ language.
/// </summary>
public record Select(Expression[] Expressions, Statement Statement) : Statement
{
    /// <summary>
    /// Accepts a visitor to process this "Select" statement.
    /// </summary>
    /// <param name="visitor">The visitor that processes this statement.</param>
    public override object Accept(IVisitor visitor)
    {
       return visitor.Visit(this);

    }
}