﻿namespace ToFLac_NEW.Model.Lexer
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
        BrokenInt = 13,
        BrokenFloat = 14,
        BrokenDouble = 15,
        BrokenChar = 16,
        BrokenNew = 17,
        BrokenIdentifier = 18,
        Invalid = 19
    }
}