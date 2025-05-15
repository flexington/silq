using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a statement that selects the last matching item in a SILQ query.
/// </summary>
public record Last(Statement Statement) : Statement
{
    /// </inheritdoc />
    public override object Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}