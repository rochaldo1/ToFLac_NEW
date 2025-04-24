namespace ToFLac_NEW.Model.Lexer
{
    public enum TokenType
    {
        Int = 1,
        Float = 2,
        Double = 3,
        Char = 4,
        Pointer = 5,
        Space = 6,
        Identifier = 7,
        Equal = 8,
        New = 9,
        LeftBracket = 10,
        RightBracket = 11,
        Semicolon = 12,
        Invalid = 13,
        BrokenType = 14,
        BrokenNew = 15,
    }
}
