using flx.SILQ.Models;

namespace flx.SILQ.Expressions;

public record Binary(Expression Left, Token Operator, Expression Right) : Expression
{
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}