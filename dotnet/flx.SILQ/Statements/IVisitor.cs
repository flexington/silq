using flx.SILQ.Expressions;

namespace flx.SILQ.Statements;

/// <summary>
/// Visitor interface for statement nodes in the SILQ AST.
/// Implementations should provide logic for each supported statement type.
/// </summary>
public interface IVisitor
{
    /// <summary>
    /// Visits a <see cref="Print"/> statement node.
    /// </summary>
    /// <param name="statement">The print statement to visit.</param>
    void Visit(Print statement);

    /// <summary>
    /// Visits a <see cref="From"/> statement node.
    /// </summary>
    /// <param name="statement">The from statement to visit.</param>
    object Visit(From statement);

    /// <summary>
    /// Visits a <see cref="Where"/> statement node.
    /// </summary>
    /// <param name="statement">The where statement to visit.</param>
    object Visit(Where statement);

    /// <summary>
    /// Visits a <see cref="Select"/> statement node.
    /// </summary>
    /// <param name="statement">The select statement to visit.</param>
    object Visit(Select statement);

    /// <summary>
    /// Visits a <see cref="As"/> statement node.
    /// </summary>
    /// <param name="statement">The as statement to visit.</param>
    void Visit(As statement);

    /// <summary>
    /// Visits a <see cref="Function"/> statement node.
    /// </summary>
    /// <param name="statement">The function statement to visit.</param>
    void Visit(Function statement);

    /// <summary>
    /// Visits a <see cref="Count"/> statement node.
    /// </summary>
    /// <param name="statement">The count statement to visit.</param>
    double Visit(Count statement);

    /// <summary>
    /// Visits a <see cref="First"/> statement node.
    /// </summary>
    /// <param name="statement">The first statement to visit.</param>
    object Visit(First statement);

    /// <summary>
    /// Visits a <see cref="Last"/> statement node.
    /// </summary>
    /// <param name="statement">The last statement to visit.</param>
    object Visit(Last statement);
}