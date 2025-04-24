namespace flx.SILQ.Expressions;

public abstract record Expression
{
    public abstract T Accpt<T>(IVisitor<T> visitor);
}
