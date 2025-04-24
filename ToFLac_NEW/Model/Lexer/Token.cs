using System.Text.RegularExpressions;

namespace ToFLac_NEW.Model.Lexer
{
    public class Token
    {
        public static Dictionary<string, (string token, TokenType tokenType)> tokens = new()
        {
            { @"^\bint\b$", ("INT", TokenType.Int) },
            { @"^\bfloat\b$", ("FLOAT", TokenType.Float) },
            { @"^\bdouble\b$", ("DOUBLE", TokenType.Double) },
            { @"^\bchar\b$", ("CHAR", TokenType.Char) },
            { @"^i[^a-zA-Z0-9\s]*n[^a-zA-Z0-9\s]*t$", ("BROKEN_INT", TokenType.BrokenType) },
            { @"^f[^a-zA-Z0-9\s]*l[^a-zA-Z0-9\s]*o[^a-zA-Z0-9\s]*a[^a-zA-Z0-9\s]*t$", ("BROKEN_FLOAT", TokenType.BrokenType) },
            { @"^d[^a-zA-Z0-9\s]*o[^a-zA-Z0-9\s]*u[^a-zA-Z0-9\s]*b[^a-zA-Z0-9\s]*l[^a-zA-Z0-9\s]*e$", ("BROKEN_DOUBLE", TokenType.BrokenType) },
            { @"^c[^a-zA-Z0-9\s]*h[^a-zA-Z0-9\s]*a[^a-zA-Z0-9\s]*r$", ("BROKEN_CHAR", TokenType.BrokenType) },
            { @"^n[^a-zA-Z0-9\s]+e[^a-zA-Z0-9\s]*w$", ("BROKEN_NEW", TokenType.BrokenNew) },
            { @"^n[^a-zA-Z0-9\s]*e[^a-zA-Z0-9\s]+w$", ("BROKEN_NEW", TokenType.BrokenNew) },
            { @"^\bnew\b$", ("NEW", TokenType.New) },
            { @"^\*$", ("POINTER", TokenType.Pointer) },
            { @"^ $", ("SPACE", TokenType.Space) },
            { @"^[a-zA-Z][a-zA-Z0-9]*$", ("IDENTIFIER", TokenType.Identifier) },
            { @"^=$", ("EQUAL", TokenType.Equal) },
            { @"^\($", ("LEFTBRACKET", TokenType.LeftBracket) },
            { @"^\)$", ("RIGHTBRACKET", TokenType.RightBracket) },
            { @"^;$", ("SEMICOLON", TokenType.Semicolon) },
            { @"^[^\s]$", ("INVALID", TokenType.Invalid) }
        };

        public int Line { get; set; }
        public int StartIdx { get; set; }
        public int EndIdx { get; set; }
        public string Message { get; set; }
        public string Terminal { get; set; }
        public string NonTerminal { get; set; }
        public TokenType TypeCode { get; set; }

        public Token(int line, int startIndex, int endIndex, string message, string terminal)
        {
            Line = line;
            StartIdx = startIndex;
            EndIdx = endIndex;
            Message = message;
            Terminal = terminal;
            var parsed = ParseToken(terminal);
            NonTerminal = parsed.Item1;
            TypeCode = parsed.Item2;
        }

        private (string, TokenType) ParseToken(string token)
        {
            foreach (var tokenReg in tokens)
            {
                if (Regex.IsMatch(token, tokenReg.Key))
                    return tokenReg.Value;
            }
            return ("INVALID", TokenType.Invalid);
        }
    }
}