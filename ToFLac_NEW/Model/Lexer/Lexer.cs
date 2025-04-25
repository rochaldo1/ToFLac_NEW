using System.Text.RegularExpressions;

namespace ToFLac_NEW.Model.Lexer
{
    public class Lexer
    {
        private const string pattern =
            @"\bint\*|\bfloat\*|\bdouble\*|\bchar\*|" +
            @"\bint\b|\bfloat\b|\bdouble\b|\bchar\b|" +
            @"\bnew\b| |=|\(|\)|;|[a-zA-Z][a-zA-Z0-9]*| |[^\s]";

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
                        case "int*":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Тип указатель: '{value}'",
                                value
                            ));
                            continue;
                        case "float*":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Тип указатель: '{value}'",
                                value
                            ));
                            continue;
                        case "double*":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Тип указатель: '{value}'",
                                value
                            ));
                            continue;
                        case "char*":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Тип указатель: '{value}'",
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
                        case "new":
                            tokens.Add(new Token(
                                lineNum + 1,
                                start,
                                pos,
                                $"Ключевое слово: '{value}'",
                                value
                            ));
                            continue;
                    }

                    switch (value)
                    {
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