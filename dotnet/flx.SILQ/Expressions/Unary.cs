using flx.SILQ.Models;

namespace flx.SILQ.Expressions;

public record Unary(Token Operator, Expression Right) : Expression
{
    public override T Accpt<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{Operator.Lexeme} {Right}";
    }
}