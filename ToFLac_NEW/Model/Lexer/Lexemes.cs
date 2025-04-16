namespace ToFLac_NEW.Model.Lexer
{
    public class Lexemes
    {
        public static Dictionary<string, TokenType> lexems = new()
        {
            { "*", TokenType.Pointer },
            { " ", TokenType.Space},
            { "=", TokenType.Equal },
            { "(", TokenType.LeftBracket },
            { ")", TokenType.RightBracket },
            { ";", TokenType.Semicolon }
        };

        public static string GetLexemes(TokenType lexeme)
        {
            foreach (var pair in lexems)
            {
                if (pair.Value == lexeme)
                    return pair.Key;
            }

            return " ";
        }
    }
}
