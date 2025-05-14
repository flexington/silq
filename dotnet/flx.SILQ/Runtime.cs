using flx.SILQ.Core;
using flx.SILQ.Errors;

namespace flx.SILQ;

public class Runtime
{
    private readonly Interpreter _interpreter;

    public Runtime(object context)
    {
        if (context == null) throw new RuntimeError("context", "Context cannot be null.");
        _interpreter = new Interpreter(context);
    }

    public object Execute(string query)
    {
        if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query)) throw new RuntimeError("query", "Query cannot be null or empty.");

        Tokenizer tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize([query]);

        Parser parser = new Parser();
        var ast = parser.ParseStatements(tokens);

        return _interpreter.Interpret(ast);
    }
}