using flx.SILQ.Models;

namespace flx.SILQ.Expressions;

/// <summary>
/// Represents a variable expression in the syntax tree.
/// </summary>
/// <param name="Name">The token representing the name of the variable.</param>
/// <param name="Member">An optional member variable, allowing for nested variable access.</param>
public record Variable(Token Name, Variable Member = null) : Expression
{
    /// <summary>
    /// Accepts a visitor to process this variable expression.
    /// </summary>
    /// <param name="visitor">The visitor that will process the variable expression.</param>
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}