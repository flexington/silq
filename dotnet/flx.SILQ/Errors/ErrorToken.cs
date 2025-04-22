namespace flx.SILQ.Errors;

public record ErrorToken(string Message, int Line, int Column)
{
    public override string ToString()
    {
        return $"[{Line}:{Column}]Error: {Message}";
    }
}