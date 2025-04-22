namespace flx.SILQ;

/// <summary>
/// Represents a single character in the source input, including its value and position (line and column).
/// </summary>
public record Character(char Value, int Line, int Column)
{
    /// <summary>
    /// Returns a string representation of the character and its position in the format: 'Value (Line, Column)'.
    /// </summary>
    public override string ToString()
    {
        return $"{Value} ({Line}, {Column})";
    }
}