namespace flx.SILQ.Statements;

/// <summary>
/// Represents a statement that selects the last matching item in a SILQ query.
/// </summary>
public record Last : Statement
{
    /// </inheritdoc />
    public override object Accept(IVisitor visitor)
    {
         visitor.Visit(this);
        return null;
    }
}