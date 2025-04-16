﻿using System.Text.RegularExpressions;

namespace ToFLac_NEW.Model.Lexer
{
    public class Lexer
    {
        private const string pattern = @"\bint\b|\bfloat\b|\bdouble\b|\bchar\b|\bnew\b|\*| |=|\(|\)|;|[a-zA-Z][a-zA-Z0-9]*| |[^\s]";

        public List<Token> GetLexemes(string text)
        {
            List<string> strings = text.Split('\n').ToList();
            List<Token> tokens = new();
            Regex regex = new Regex(pattern);

            for (int i = 0; i < strings.Count; i++)
            {
                MatchCollection matches = regex.Matches(strings[i]);

                foreach (Match match in matches)
                {
                    switch (match.Value)
                    {
                        case "int":
                        case "float":
                        case "double":
                        case "char":
                            tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                $"Найдено ключевое слово типа: '{match.Value}'", match.Value));
                            break;

                        case "new":
                            tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                $"Найдено ключевое слово: '{match.Value}'", match.Value));
                            break;

                        case "*":
                            tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                $"Найден указатель: '{match.Value}'", match.Value));
                            break;

                        case " ":
                            tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                $"Найден разделитель: '{match.Value}'", match.Value));
                            break;

                        case "=":
                            tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                $"Найден оператор присваивания: '{match.Value}'", match.Value));
                            break;

                        case "(":
                            tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                $"Найдена открывающая скобка: '{match.Value}'", match.Value));
                            break;

                        case ")":
                            tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                $"Найдена закрывающая скобка: '{match.Value}'", match.Value));
                            break;

                        case ";":
                            tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                $"Найден символ конца строки: '{match.Value}'", match.Value));
                            break;

                        default:
                            if (Regex.IsMatch(match.Value, @"^[a-zA-Z][a-zA-Z0-9]*$"))
                            {
                                tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                    $"Найден идентификатор: '{match.Value}'", match.Value));
                            }
                            else
                            {
                                tokens.Add(new Token(i + 1, match.Index, match.Length + match.Index,
                                    $"Найден недопустимый символ: '{match.Value}'", match.Value));
                            }
                            break;
                    }
                }
            }

            return tokens;
        }
    }
}
