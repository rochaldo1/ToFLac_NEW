namespace ToFLac_NEW.Model.Lexer
{
    public static class Lexeme
    {
        public static Dictionary<string, TokenType> Lexemes = new Dictionary<string, TokenType>
        {
            { "new", TokenType.New},
            { "=", TokenType.Equal },
            { " ", TokenType.Space },
            { ";", TokenType.Semicolon },
            { "(", TokenType.LeftBracket },
            { ")", TokenType.RightBracket }
        };

        public static string GetLexeme(TokenType lexeme)
        {
            foreach (var kvp in Lexemes)
            {
                if (kvp.Value == lexeme)
                    return kvp.Key;
            }

            return " ";
        }
    }
}
