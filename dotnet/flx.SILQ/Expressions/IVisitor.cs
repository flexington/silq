namespace flx.SILQ.Expressions;

public interface IVisitor<T>
{
    T Visit(Literal literal);
    T Visit(Unary unary);
    T Visit(Binary binary);
    T Visit(Grouping grouping);
    T Visit(Variable variable);
    T Visit(Function function);
    T Visit(Logical logical);
}