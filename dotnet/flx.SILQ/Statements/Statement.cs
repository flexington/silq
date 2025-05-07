namespace flx.SILQ.Statements;

/// <summary>
/// Represents the abstract base class for all statement nodes in the SILQ abstract syntax tree (AST).
/// </summary>
public abstract record Statement
{
    /// <summary>
    /// Accepts a visitor that processes this statement node.
    /// </summary>
    /// <param name="visitor">The visitor to process the statement.</param>
    public abstract void Accept(IVisitor visitor);
}