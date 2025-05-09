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
    void Visit(From statement);

    /// <summary>
    /// Visits a <see cref="Where"/> statement node.
    /// </summary>
    /// <param name="statement">The where statement to visit.</param>
    void Visit(Where statement);

    /// <summary>
    /// Visits a <see cref="Select"/> statement node.
    /// </summary>
    /// <param name="statement">The select statement to visit.</param>
    void Visit(Select statement);

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
}