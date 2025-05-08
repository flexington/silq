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
    /// <param name="print">The print statement to visit.</param>
    void Visit(Print print);

    /// <summary>
    /// Visits a <see cref="From"/> statement node.
    /// </summary>
    /// <param name="from">The from statement to visit.</param>
    void Visit(From from);

    /// <summary>
    /// Visits a <see cref="Where"/> statement node.
    /// </summary>
    /// <param name="where">The where statement to visit.</param>
    void Visit(Where where);
}