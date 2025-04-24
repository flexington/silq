using flx.SILQ.Models;

namespace flx.SILQ.Expressions;

public record Binary(Expression Left, Token Operator, Expression Right) : Expression
{
    public override T Accpt<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"({Left} {Operator.Lexeme} {Right})";
    }
}