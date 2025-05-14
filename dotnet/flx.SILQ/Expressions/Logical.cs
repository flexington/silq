using flx.SILQ.Models;

namespace flx.SILQ.Expressions;

public record Logical(Expression Left, Token Op, Expression right) : Expression
{
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}