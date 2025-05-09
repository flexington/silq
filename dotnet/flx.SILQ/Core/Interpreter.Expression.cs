using System;
using flx.SILQ.Expressions;
using flx.SILQ.Models;

namespace flx.SILQ.Core;

/// <summary>
/// Implements the visitor pattern for evaluating expressions in the SILQ interpreter.
/// </summary>
public partial class Interpreter : IVisitor<object>
{
    /// <summary>
    /// Visits a <see cref="Literal"/> expression and returns its value, converting to double if possible.
    /// </summary>
    /// <param name="literal">The literal expression to evaluate.</param>
    /// <returns>The value of the literal, possibly as a double.</returns>
    public object Visit(Literal literal)
    {
        if (CanConvertToDouble(literal.Value)) return Convert.ToDouble(literal.Value);

        return literal.Value;
    }

    /// <summary>
    /// Visits a <see cref="Unary"/> expression and evaluates it according to its operator.
    /// </summary>
    /// <param name="unary">The unary expression to evaluate.</param>
    /// <returns>The result of the unary operation.</returns>
    public object Visit(Unary unary)
    {
        object right = Evaluate(unary.Right);

        switch (unary.Operator.TokenType)
        {
            case TokenType.BANG:
                return !IsTruthy(right);
            case TokenType.MINUS:
                CheckNumberOperands(unary.Operator, right);
                return -Convert.ToDouble(right);
        }

        return null;
    }

    /// <summary>
    /// Visits a <see cref="Binary"/> expression and evaluates it according to its operator and operands.
    /// </summary>
    /// <param name="binary">The binary expression to evaluate.</param>
    /// <returns>The result of the binary operation.</returns>
    public object Visit(Binary binary)
    {
        object left = Evaluate(binary.Left);
        object right = Evaluate(binary.Right);

        switch (binary.Operator.TokenType)
        {
            case TokenType.GREATER:
                CheckNumberOperands(binary.Operator, left, right);
                return Convert.ToDouble(left) > Convert.ToDouble(right);
            case TokenType.GREATER_EQUAL:
                CheckNumberOperands(binary.Operator, left, right);
                return Convert.ToDouble(left) >= Convert.ToDouble(right);
            case TokenType.LESS:
                CheckNumberOperands(binary.Operator, left, right);
                return Convert.ToDouble(left) < Convert.ToDouble(right);
            case TokenType.LESS_EQUAL:
                CheckNumberOperands(binary.Operator, left, right);
                return Convert.ToDouble(left) <= Convert.ToDouble(right);
            case TokenType.BANG_EQUAL: return !IsEqual(left, right);
            case TokenType.EQUAL_EQUAL: return IsEqual(left, right);
            case TokenType.MINUS:
                CheckNumberOperands(binary.Operator, left, right);
                return Convert.ToDouble(left) - Convert.ToDouble(right);
            case TokenType.PLUS: return Add(binary.Operator, left, right);
            case TokenType.SLASH:
                CheckNumberOperands(binary.Operator, left, right);
                return Convert.ToDouble(left) / Convert.ToDouble(right);
            case TokenType.STAR:
                CheckNumberOperands(binary.Operator, left, right);
                return Convert.ToDouble(left) * Convert.ToDouble(right);

        }
        return null;
    }

    /// <summary>
    /// Visits a <see cref="Grouping"/> expression and evaluates its contained expression.
    /// </summary>
    /// <param name="grouping">The grouping expression to evaluate.</param>
    /// <returns>The result of the evaluated grouping expression.</returns>
    public object Visit(Grouping grouping)
    {
       return Evaluate(grouping.Expression);
    }

    /// <summary>
    /// Visits a <see cref="Variable"/> expression and retrieves its value from the environment.
    /// </summary>
    /// <param name="variable">The variable expression to evaluate.</param>
    /// <returns>The value of the variable from the environment.</returns>
    public object Visit(Variable variable)
    {
        // return _environment.Get(variable.Name);
        return null; // Placeholder for actual environment retrieval
    }

    /// <summary>
    /// Visits a <see cref="Function"/> expression and processes its behavior.
    /// </summary>
    /// <param name="function">The function expression to evaluate.</param>
    /// <returns>The result of the function evaluation.</returns>
    public object Visit(Function function)
    {
        throw new NotImplementedException();
    }
}