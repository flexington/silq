using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a statement that selects the first matching item in a SILQ query.
/// </summary>
public record First(Statement Statement) : Statement
{
    /// </inheritdoc />
    public override object Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}