using System.Text.RegularExpressions;

namespace ToFLac_NEW.Model.Lexer
{
    public class Token
    {
        public static Dictionary<string, (string token, TokenType tokenType)> tokens = new()
        {
            { @"^int$", ("INT", TokenType.Int) },
            { @"^float$", ("FLOAT", TokenType.Float) },
            { @"^double$", ("DOUBLE", TokenType.Double) },
            { @"^char$", ("CHAR", TokenType.Char) },
            { @"^new$", ("NEW", TokenType.New) },
            { @"\*", ("POINTER", TokenType.Pointer) },
            { @"^ $", ("SPACE", TokenType.Space) },
            { @"^[a-zA-Z][a-zA-Z0-9]*$", ("IDENTIFIER", TokenType.Identifier) },
            { @"^=$", ("EQUAL", TokenType.Equal) },
            { @"^\($", ("LEFT_BRACKET", TokenType.LeftBracket) },
            { @"^\)$", ("RIGHT_BRACKET", TokenType.RightBracket) },
            { @"^;$", ("SEMICOLON", TokenType.Semicolon) },

            { @"^i[^n\w\s]?n[^t\w\s]?t$|^i[^n\w\s]?nt$|^in[^t\w\s]?t$|^i[^n\w\s]?n[^t\w\s]?t$", ("BROKEN_INT", TokenType.BrokenInt) },
            { @"^f[^l\w\s]?l[^o\w\s]?o[^a\w\s]?a[^t\w\s]?t$|^f[^l\w\s]?loat$|^fl[^o\w\s]?oat$|^flo[^a\w\s]?at$|^floa[^t\w\s]?t$|^f[^l\w\s]?l[^o\w\s]?o[^a\w\s]?a[^t\w\s]?t$", ("BROKEN_FLOAT", TokenType.BrokenFloat) },
            { @"^d[^o\w\s]?o[^u\w\s]?u[^b\w\s]?b[^l\w\s]?l[^e\w\s]?e$|^d[^o\w\s]?ouble$|^do[^u\w\s]?uble$|^dou[^b\w\s]?ble$|^doub[^l\w\s]?le$|^doubl[^e\w\s]?e$|^d[^o\w\s]?o[^u\w\s]?u[^b\w\s]?b[^l\w\s]?l[^e\w\s]?e$", ("BROKEN_DOUBLE", TokenType.BrokenDouble) },
            { @"^c[^h\w\s]?h[^a\w\s]?a[^r\w\s]?r$|^c[^h\w\s]?har$|^ch[^a\w\s]?ar$|^cha[^r\w\s]?r$|^c[^h\w\s]?h[^a\w\s]?a[^r\w\s]?r$", ("BROKEN_CHAR", TokenType.BrokenChar) },
            { @"^n[^e\w\s]?e[^w\w\s]?w$|^n[^e\w\s]?ew$|^ne[^w\w\s]?w$|^n[^e\w\s]?e[^w\w\s]?w$", ("BROKEN_NEW", TokenType.BrokenNew) },
            { @"^[a-zA-Z][a-zA-Z0-9]*([^a-zA-Z0-9\s][a-zA-Z0-9]*)+$", ("BROKEN_IDENTIFIER", TokenType.BrokenIdentifier) },

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
