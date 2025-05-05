using System.Text.RegularExpressions;

namespace ToFLac_NEW.Model.Lexer
{
    public class Lexer
    {
        private const string pattern =
            @"(int|i[^n\w\s]?nt|in[^t\w\s]?t|i[^n\w\s]?n[^t\w\s]?t)|" +
            @"(float|f[^l\w\s]?loat|fl[^o\w\s]?oat|flo[^a\w\s]?at|floa[^t\w\s]?t|f[^l\w\s]?l[^o\w\s]?o[^a\w\s]?a[^t\w\s]?t)|" +
            @"(double|d[^o\w\s]?ouble|do[^u\w\s]?uble|dou[^b\w\s]?ble|doub[^l\w\s]?le|doubl[^e\w\s]?e|d[^o\w\s]?o[^u\w\s]?u[^b\w\s]?b[^l\w\s]?l[^e\w\s]?e)|" +
            @"(char|c[^h\w\s]?har|ch[^a\w\s]?ar|cha[^r\w\s]?r|c[^h\w\s]?h[^a\w\s]?a[^r\w\s]?r)|" +
            @"(new|n[^e\w\s]?ew|ne[^w\w\s]?w|n[^e\w\s]?e[^w\w\s]?w)|" +
            @"\*|" +
            @"=|\(|\)|;|" +
            @"[a-zA-Z][a-zA-Z0-9]*|" +
            @"\s+|" +
            @"[^\s]";

        public List<Token> GetLexemes(string text)
        {
            List<Token> tokens = new();
            string[] lines = text.Split('\n');

            for (int lineNum = 0; lineNum < lines.Length; lineNum++)
            {
                string line = lines[lineNum];
                int pos = 0;

                while (pos < line.Length)
                {
                    Match match = Regex.Match(line.Substring(pos), @"^(" + pattern + ")");
                    if (!match.Success)
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            pos,
                            pos + 1,
                            $"Недопустимый символ: '{line[pos]}'",
                            line[pos].ToString()
                        ));
                        pos++;
                        continue;
                    }

                    string value = match.Value;
                    int start = pos;
                    pos += value.Length;

                    if (Regex.IsMatch(value, @"^int$|^float$|^double$|^char$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Тип: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^new$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Ключевое слово: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^\*$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Указатель: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^=$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Знак равенства: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^\($"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Открывающая скобка: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^\)$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Закрывающая скобка: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^;$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Точка с запятой: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^\s+$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Пробел: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^[a-zA-Z][a-zA-Z0-9]*$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Идентификатор: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^i[^n\w\s]?nt$|^in[^t\w\s]?t$|^i[^n\w\s]?n[^t\w\s]?t$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Сломанный int: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^f[^l\w\s]?loat$|^fl[^o\w\s]?oat$|^flo[^a\w\s]?at$|^floa[^t\w\s]?t$|^f[^l\w\s]?l[^o\w\s]?o[^a\w\s]?a[^t\w\s]?t$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Сломанный float: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^d[^o\w\s]?ouble$|^do[^u\w\s]?uble$|^dou[^b\w\s]?ble$|^doub[^l\w\s]?le$|^doubl[^e\w\s]?e$|^d[^o\w\s]?o[^u\w\s]?u[^b\w\s]?b[^l\w\s]?l[^e\w\s]?e$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Сломанный double: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^c[^h\w\s]?har$|^ch[^a\w\s]?ar$|^cha[^r\w\s]?r$|^c[^h\w\s]?h[^a\w\s]?a[^r\w\s]?r$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Сломанный char: '{value}'",
                            value
                        ));
                    }
                    else if (Regex.IsMatch(value, @"^n[^e\w\s]?ew$|^ne[^w\w\s]?w$|^n[^e\w\s]?e[^w\w\s]?w$"))
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Сломанный new: '{value}'",
                            value
                        ));
                    }
                    else
                    {
                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Недопустимый токен: '{value}'",
                            value
                        ));
                    }
                }
            }

            tokens = MergeBrokenIdentifiers(tokens);
            tokens = MergeTypeAndIdentifiers(tokens);
            return tokens;
        }

        private List<Token> MergeBrokenIdentifiers(List<Token> tokens)
        {
            List<Token> mergedTokens = new();
            int i = 0;

            while (i < tokens.Count)
            {
                if (tokens[i].TypeCode == TokenType.Identifier)
                {
                    int startIndex = i;
                    int endIndex = i;

                    if (endIndex + 2 < tokens.Count &&
                        tokens[endIndex + 1].TypeCode == TokenType.Invalid &&
                        tokens[endIndex + 2].TypeCode == TokenType.Identifier)
                    {
                        endIndex += 2;

                        Token mergedToken = Token.MergeTokens(tokens, startIndex, endIndex);
                        mergedTokens.Add(mergedToken);
                        i = endIndex + 1;

                        if (i < tokens.Count && tokens[i].TypeCode == TokenType.Invalid)
                        {
                            mergedTokens.Add(tokens[i]);
                            i++;
                        }
                    }
                    else
                    {
                        mergedTokens.Add(tokens[i]);
                        i++;
                    }
                }
                else
                {
                    mergedTokens.Add(tokens[i]);
                    i++;
                }
            }

            return mergedTokens;
        }

        private List<Token> MergeTypeAndIdentifiers(List<Token> tokens)
        {
            List<Token> mergedTokens = new List<Token>();
            int i = 0;

            while (i < tokens.Count)
            {
                if (IsTypeToken(tokens[i].TypeCode))
                {
                    if (i + 1 < tokens.Count && tokens[i + 1].TypeCode == TokenType.Identifier)
                    {
                        if (!HasSpaceBetween(tokens, i, i + 1))
                        {
                            Token mergedToken = MergeTypeAndIdentifier(tokens[i], tokens[i + 1]);
                            mergedTokens.Add(mergedToken);
                            i += 2;

                            if (i < tokens.Count && tokens[i].TypeCode == TokenType.Identifier &&
                                !HasSpaceBetween(mergedTokens, mergedTokens.Count - 1, i))
                            {
                                Token prevMerged = mergedTokens[mergedTokens.Count - 1];
                                Token newMerged = MergeTypeAndIdentifier(prevMerged, tokens[i]);
                                mergedTokens[mergedTokens.Count - 1] = newMerged;
                                i++;
                            }
                        }
                        else
                        {
                            mergedTokens.Add(tokens[i]);
                            i++;
                        }
                    }
                    else
                    {
                        mergedTokens.Add(tokens[i]);
                        i++;
                    }
                }
                else
                {
                    mergedTokens.Add(tokens[i]);
                    i++;
                }
            }

            return mergedTokens;
        }

        private bool IsTypeToken(TokenType type)
        {
            return type == TokenType.Int || type == TokenType.Float || type == TokenType.Double || type == TokenType.Char ||
                   type == TokenType.BrokenInt || type == TokenType.BrokenFloat || type == TokenType.BrokenDouble || type == TokenType.BrokenChar;
        }

        private bool HasSpaceBetween(List<Token> tokens, int firstIndex, int secondIndex)
        {
            if (secondIndex - firstIndex > 1)
            {
                for (int i = firstIndex + 1; i < secondIndex; i++)
                {
                    if (tokens[i].TypeCode == TokenType.Space)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Token MergeTypeAndIdentifier(Token typeToken, Token identifierToken)
        {
            string mergedTerminal = typeToken.Terminal + identifierToken.Terminal;
            int mergedStartIdx = typeToken.StartIdx;
            int mergedEndIdx = identifierToken.EndIdx;
            int mergedLine = typeToken.Line;

            bool isBrokenType = typeToken.TypeCode == TokenType.BrokenInt ||
                               typeToken.TypeCode == TokenType.BrokenFloat ||
                               typeToken.TypeCode == TokenType.BrokenDouble ||
                               typeToken.TypeCode == TokenType.BrokenChar;

            bool hasInvalidChars = identifierToken.TypeCode == TokenType.BrokenIdentifier ||
                                 identifierToken.TypeCode == TokenType.Invalid;

            if (isBrokenType || hasInvalidChars)
            {
                return new Token(mergedLine, mergedStartIdx, mergedEndIdx, $"Сломанный идентификатор: '{mergedTerminal}'", mergedTerminal)
                {
                    NonTerminal = "BROKEN_IDENTIFIER",
                    TypeCode = TokenType.BrokenIdentifier
                };
            }
            else
            {
                return new Token(mergedLine, mergedStartIdx, mergedEndIdx, $"Идентификатор: '{mergedTerminal}'", mergedTerminal)
                {
                    NonTerminal = "IDENTIFIER",
                    TypeCode = TokenType.Identifier
                };
            }
        }
    }
}
