using System.Collections.Generic;
using flx.SILQ.Models;

namespace flx.SILQ.Expressions;

public record Function(Token Name, List<Expression> Arguments) : Expression
{
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}