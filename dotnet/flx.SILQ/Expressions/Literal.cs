namespace flx.SILQ.Expressions;

public record Literal(object Value) : Expression
{
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}