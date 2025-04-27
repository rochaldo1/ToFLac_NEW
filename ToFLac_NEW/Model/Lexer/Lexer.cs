using System.Text.RegularExpressions;
using ToFLac_NEW.Model.Lexer;

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
        @"[a-zA-Z][a-zA-Z0-9]*([^a-zA-Z0-9\s][a-zA-Z0-9]*)*|" +
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
                else if (Regex.IsMatch(value, @"^[a-zA-Z][a-zA-Z0-9]*([^a-zA-Z0-9\s][a-zA-Z0-9]*)+$"))
                {
                    tokens.Add(new Token(
                        lineNum + 1,
                        start,
                        pos,
                        $"Сломанный идентификатор: '{value}'",
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

        return tokens;
    }
}
