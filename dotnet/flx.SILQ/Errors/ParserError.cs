using System;
using flx.SILQ.Models;

namespace flx.SILQ.Errors;

public class ParserError : Exception
{
    private readonly Token _token;
    public Token Token => _token;

    public ParserError(Token token, string message) : base(message)
    {
        _token = token;
    }
}