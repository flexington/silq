using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Represents a statement that counts the number of matching items in a SILQ query.
/// </summary>
public record Count : Statement
{

    public Expression Expression { get; }

    public Statement Statement { get; }

    public Count(Expression expression)
    {
        Expression = expression;
    }


    public Count(Statement statement)
    {
        Statement = statement;
    }

    /// </inheritdoc />
    public override object Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}