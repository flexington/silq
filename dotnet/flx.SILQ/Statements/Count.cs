namespace flx.SILQ.Statements;

/// <summary>
/// Represents a statement that counts the number of matching items in a SILQ query.
/// </summary>
public record Count : Statement
{
    /// </inheritdoc />
    public override object Accept(IVisitor visitor)
    {
         visitor.Visit(this);
        return null;
    }
}