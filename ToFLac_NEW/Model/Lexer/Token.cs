using System.Text.RegularExpressions;

namespace ToFLac_NEW.Model.Lexer
{
    public class Token
    {
        public static Dictionary<string, (string token, TokenType tokenType)> tokens = new()
        {
            { @"\bint\b", ("INT", TokenType.Int) },
            { @"\bfloat\b", ("FLOAT", TokenType.Float) },
            { @"\bdouble\b", ("DOUBLE", TokenType.Double) },
            { @"\bchar\b", ("CHAR", TokenType.Char) },
            { @"\*", ("POINTER", TokenType.Pointer) },
            { @" ", ("SPACE", TokenType.Space) },
            { @"[a-zA-Z][a-zA-Z0-9]*", ("IDENTIFIER", TokenType.Identifier) },
            { @"=", ("EQUAL", TokenType.Equal) },
            { @"\bnew\b", ("NEW", TokenType.New) },
            { @"\(", ("LEFTBRACKET", TokenType.LeftBracket) },
            { @"\)", ("RIGHTBRACKET", TokenType.RightBracket) },
            { @";", ("SEMICOLON", TokenType.Semicolon) },
            { @"[^\s]", ("INVALID", TokenType.Invalid) }
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
            NonTerminal = ParseToken(terminal).Item1;
            TypeCode = ParseToken(terminal).Item2;
        }

        private (string, TokenType) ParseToken(string token)
        {
            foreach (var tokenReg in tokens)
            {
                if (Regex.IsMatch(token, tokenReg.Key))
                    return tokenReg.Value;
            }
            return("INVALID", TokenType.Invalid);
        }
    }
}
