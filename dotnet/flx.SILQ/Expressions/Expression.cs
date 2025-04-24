namespace flx.SILQ.Expressions;

public abstract record Expression
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}
