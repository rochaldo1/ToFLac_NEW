namespace ToFLac_NEW.Model.Lexer
{
    public enum TokenType
    {
        IntPointer = 1,
        FloatPointer = 2,
        DoublePointer = 3,
        CharPointer = 4,
        Int = 5,
        Float = 6,
        Double = 7,
        Char = 8,
        Space = 9,
        Identifier = 10,
        Equal = 11,
        New = 12,
        LeftBracket = 13,
        RightBracket = 14,
        Semicolon = 15,
        Invalid = 16
    }
}