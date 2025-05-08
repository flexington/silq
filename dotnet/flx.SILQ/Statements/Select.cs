using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a "Select" statement in the SILQ language.
/// </summary>
public record Select(Expression[] Expressions) : Statement
{
    /// <summary>
    /// Accepts a visitor to process this "Select" statement.
    /// </summary>
    /// <param name="visitor">The visitor that processes this statement.</param>
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}