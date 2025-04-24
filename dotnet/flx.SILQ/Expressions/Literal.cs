namespace flx.SILQ.Expressions;

public record Literal(object Value) : Expression
{
    public override T Accpt<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}