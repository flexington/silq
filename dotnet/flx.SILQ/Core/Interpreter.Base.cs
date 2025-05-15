using System;
using System.Collections;
using System.Collections.Generic;
using flx.SILQ.Errors;
using flx.SILQ.Expressions;
using flx.SILQ.Models;
using flx.SILQ.Statements;

namespace flx.SILQ.Core;

/// <summary>
/// Provides core evaluation logic for the SILQ interpreter, including arithmetic operations, type checking, and expression evaluation.
/// </summary>
public partial class Interpreter
{
    private Environment _environment = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Interpreter"/> class with the specified context.
    /// </summary>
    /// <param name="context">The context in which the interpreter operates.</param>
    public Interpreter(object context)
    {
        if (context == null) throw new RuntimeError("context", "Context cannot be null.");
        _environment.SetContext(context);
    }

    /// <summary>
    /// Interprets the given expression by evaluating it and converting the result to a string.
    /// </summary>
    /// <param name="expression">The expression to interpret.</param>
    /// <returns>The string representation of the evaluated expression.</returns>
    public string Interpret(Expression expression)
    {
        try
        {
            object value = Evaluate(expression);
            return Stringify(value);
        }
        catch (RuntimeError)
        {
            throw;
        }
    }

    /// <summary>
    /// Interprets a list of statements by executing each statement in order.
    /// Throws a <see cref="RuntimeError"/> if an error occurs during execution.
    /// </summary>
    /// <param name="statements">The list of statements to interpret.</param>
    public object Interpret(List<Statement> statements)
    {
        try
        {
            object result = null;
            foreach (var statement in statements)
            {
                result = Execute(statement);
            }

            return result;
        }
        catch (RuntimeError)
        {
            throw;
        }
    }

    /// <summary>
    /// Adds two operands, supporting both numeric addition and string concatenation.
    /// Throws a <see cref="RuntimeError"/> if operands are not both numbers or both strings.
    /// </summary>
    /// <param name="token">The operator token for error reporting.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The sum or concatenation of the operands.</returns>
    private object Add(Token token, object left, object right)
    {
        if (CanConvertToDouble(left) && CanConvertToDouble(right)) return Convert.ToDouble(left) + Convert.ToDouble(right);
        if (left is string && right is string) return (string)left + (string)right;
        throw new RuntimeError(token, "Operands must be two numbers or two strings.");
    }

    /// <summary>
    /// Determines if an object can be converted to a double (numeric type).
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>True if the object can be converted to double; otherwise, false.</returns>
    private bool CanConvertToDouble(object obj)
    {
        if (obj == null) return false;
        if (!IsNumeric(obj)) return false;

        try
        {
            Convert.ToDouble(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if the operand is a number, throwing a <see cref="RuntimeError"/> if not.
    /// </summary>
    /// <param name="operator">The operator token for error reporting.</param>
    /// <param name="operand">The operand to check.</param>
    private void CheckNumberOperands(Token @operator, object operand)
    {
        if (CanConvertToDouble(operand)) return;
        throw new RuntimeError(@operator, $"Operand must be a number.");
    }

    /// <summary>
    /// Checks if both operands are numbers, throwing a <see cref="RuntimeError"/> if not.
    /// </summary>
    /// <param name="operator">The operator token for error reporting.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    private void CheckNumberOperands(Token @operator, object left, object right)
    {
        if (CanConvertToDouble(left) && CanConvertToDouble(right)) return;
        throw new RuntimeError(@operator, "Operands must be numbers.");
    }

    /// <summary>
    /// Evaluates an expression by accepting this interpreter as a visitor.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <returns>The result of evaluating the expression.</returns>
    private object Evaluate(Expression expression)
    {
        return expression.Accept(this);
    }

    /// <summary>
    /// Executes a statement by accepting this interpreter as a visitor.
    /// </summary>
    /// <param name="statement">The statement to execute.</param>
    private object Execute(Statement statement)
    {
        return statement.Accept(this);
    }

    /// <summary>
    /// Checks if two objects are equal, handling nulls and type differences.
    /// </summary>
    /// <param name="left">The left object.</param>
    /// <param name="right">The right object.</param>
    /// <returns>True if the objects are equal; otherwise, false.</returns>
    private bool IsEqual(object left, object right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;
        var typeOne = left.GetType();
        var typeTwo = right.GetType();
        var equal = left.Equals(right);
        return (left).Equals(right);
    }

    /// <summary>
    /// Determines if an object is a numeric type (double, int, float, long, short, or byte).
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>True if the object is numeric; otherwise, false.</returns>
    private bool IsNumeric(object obj)
    {
        if (obj == null) return false;

        if (obj is double) return true;
        if (obj is int) return true;
        if (obj is float) return true;
        if (obj is long) return true;
        if (obj is short) return true;
        if (obj is byte) return true;

        return false;
    }

    /// <summary>
    /// Determines the truthiness of an object for logical operations.
    /// Returns false for null, the boolean value for bools, and true otherwise.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>True if the object is considered true; otherwise, false.</returns>
    private bool IsTruthy(object obj)
    {
        if (obj == null) return false;
        if (obj is bool) return (bool)obj;
        return true;
    }

    /// <summary>
    /// Converts an object to its string representation for the interpreter.
    /// Returns "null" for null, lower-case for booleans, and trims ".0" for whole numbers.
    /// </summary>
    /// <param name="obj">The object to stringify.</param>
    /// <returns>The string representation of the object.</returns>
    private string Stringify(object obj)
    {
        if (obj == null) return "null";
        if (obj is bool) return obj.ToString().ToLower();

        if (CanConvertToDouble(obj))
        {
            var text = Convert.ToDouble(obj).ToString();
            if (text.EndsWith(".0")) text = text.Substring(0, text.Length - 2);
            return text;
        }
        return obj.ToString();
    }

    /// <summary>
    /// Resolves a variable in the current context, allowing for nested member access.
    /// </summary>
    /// <param name="variable">The variable to resolve.</param>
    /// <param name="context">The context in which to resolve the variable.</param>
    /// <returns>The resolved value of the variable.</returns>
    /// <exception cref="RuntimeError">Thrown if the variable is not defined in the context.</exception>
    public object ResolveVariable(Variable variable, object context)
    {
        var property = context.GetType().GetProperty(variable.Name.Lexeme);
        if (property == null) throw new RuntimeError(variable.Name, $"The identifier '{variable.Name.Lexeme}' is not defined in the current context.");

        var propertyValue = property.GetValue(context);

        if (variable.Member is not null)
        {
            return ResolveVariable(variable.Member, propertyValue);
        }

        if (CanConvertToDouble(propertyValue)) propertyValue = Convert.ToDouble(propertyValue);

        return propertyValue;
    }

    public object GetContext()
    {
        return _environment.Get("context");
    }

    /// <summary>
    /// Checks if the given object is a list.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    private bool IsList(object obj)
    {
        // Check if the object implements the IList interface
        return obj is IList;
    }
}