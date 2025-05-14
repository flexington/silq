using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a statement that selects the first matching item in a SILQ query.
/// </summary>
public record First() : Statement
{
    /// </inheritdoc />
    public override object Accept(IVisitor visitor)
    {
        visitor.Visit(this);
        return null;
    }
}