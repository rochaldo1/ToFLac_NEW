using System.Text.RegularExpressions;

namespace ToFLac_NEW.Model.Lexer
{
    public class Lexer
    {
        private const string pattern =
            @"(i[^a-zA-Z0-9\s]*n[^a-zA-Z0-9\s]*t)|" +
            @"(f[^a-zA-Z0-9\s]*l[^a-zA-Z0-9\s]*o[^a-zA-Z0-9\s]*a[^a-zA-Z0-9\s]*t)|" +
            @"(d[^a-zA-Z0-9\s]*o[^a-zA-Z0-9\s]*u[^a-zA-Z0-9\s]*b[^a-zA-Z0-9\s]*l[^a-zA-Z0-9\s]*e)|" +
            @"(c[^a-zA-Z0-9\s]*h[^a-zA-Z0-9\s]*a[^a-zA-Z0-9\s]*r)|" +
            @"(n[^a-zA-Z0-9\s]+e[^a-zA-Z0-9\s]*w|n[^a-zA-Z0-9\s]*e[^a-zA-Z0-9\s]+w)|" +
            @"\bint\b|\bfloat\b|\bdouble\b|\bchar\b|\bnew\b|\*| |=|\(|\)|;|[a-zA-Z][a-zA-Z0-9]*| |[^\s]";

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

                    switch (value)
                    {
                        case "new":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Ключевое слово: '{value}'",
                                value
                            ));
                            continue;

                        case "int":
                        case "float":
                        case "double":
                        case "char":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Тип: '{value}'",
                                value
                            ));
                            continue;
                    }

                    bool isBrokenType = Regex.IsMatch(value, @"^(i[^a-zA-Z0-9\s]*n[^a-zA-Z0-9\s]*t)$") ||
                                        Regex.IsMatch(value, @"^(f[^a-zA-Z0-9\s]*l[^a-zA-Z0-9\s]*o[^a-zA-Z0-9\s]*a[^a-zA-Z0-9\s]*t)$") ||
                                        Regex.IsMatch(value, @"^(d[^a-zA-Z0-9\s]*o[^a-zA-Z0-9\s]*u[^a-zA-Z0-9\s]*b[^a-zA-Z0-9\s]*l[^a-zA-Z0-9\s]*e)$") ||
                                        Regex.IsMatch(value, @"^(c[^a-zA-Z0-9\s]*h[^a-zA-Z0-9\s]*a[^a-zA-Z0-9\s]*r)$");

                    bool isBrokenNew = Regex.IsMatch(value, @"^(n[^a-zA-Z0-9\s]+e[^a-zA-Z0-9\s]*w|n[^a-zA-Z0-9\s]*e[^a-zA-Z0-9\s]+w)$");

                    if (isBrokenType || isBrokenNew)
                    {
                        string errorType = isBrokenNew ? "NEW" :
                                         value.StartsWith("i") ? "INT" :
                                         value.StartsWith("f") ? "FLOAT" :
                                         value.StartsWith("d") ? "DOUBLE" : "CHAR";

                        tokens.Add(new Token(
                            lineNum + 1,
                            start,
                            pos,
                            $"Некорректное ключевое слово {errorType}: '{value}'",
                            value
                        ));
                        continue;
                    }

                    switch (value)
                    {
                        case "*":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Указатель: '{value}'",
                                value
                            ));
                            break;

                        case "=":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Оператор присваивания: '{value}'",
                                value
                            ));
                            break;

                        case "(":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Открывающая скобка: '{value}'",
                                value
                            ));
                            break;

                        case ")":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Закрывающая скобка: '{value}'",
                                value
                            ));
                            break;

                        case ";":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Конец строки: '{value}'",
                                value
                            ));
                            break;

                        case " ":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                "Пробел",
                                value
                            ));
                            break;

                        default:
                            if (Regex.IsMatch(value, @"^[a-zA-Z][a-zA-Z0-9]*$"))
                            {
                                tokens.Add(new Token(
                                    lineNum + 1,
                                    start,
                                    pos,
                                    $"Идентификатор: '{value}'",
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
                            break;
                    }
                }
            }

            return tokens;
        }
    }
}