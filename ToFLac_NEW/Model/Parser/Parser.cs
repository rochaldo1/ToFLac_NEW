using ToFLac_NEW.Model.Lexer;

namespace ToFLac_NEW.Model.Parser
{
    public class Parser
    {
        private readonly List<ErrorToken> _errors = new();
        private List<List<Token>> _tokens = new();

        public List<ErrorToken> Parse(List<Token> inputTokens)
        {
            _errors.Clear();
            _tokens.Clear();
            
            GroupTokensByLine(inputTokens);
            
            foreach (var lineTokens in _tokens)
            {
                var lineParser = new LineTokenRecursiveParser(lineTokens);
                _errors.AddRange(lineParser.Parse());
            }

            return _errors;
        }

        private void GroupTokensByLine(List<Token> tokens)
        {
            _tokens = tokens
                .GroupBy(token => token.Line)
                .Select(group => group.ToList())
                .ToList();
        }
    }

    public partial class LineTokenRecursiveParser
    {
        private readonly List<Token> _tokens;

        public LineTokenRecursiveParser(List<Token> tokens)
        {
            _tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
        }

        public List<ErrorToken> Parse()
        {
            int currentPosition = 0;
            List<ErrorToken> errors = new();
            return ParseStart(currentPosition, errors);
        }

        private List<ErrorToken> ParseStart(int currentPosition,  List<ErrorToken> errors)
        {
            if (currentPosition >= _tokens.Count)
                return errors;

            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (_tokens[currentPosition].TypeCode == TokenType.Space)
                return ParseStart(currentPosition + 1, errors);

            var currentType = _tokens[currentPosition].TypeCode;

            if (currentType == TokenType.IntPointer ||
                currentType == TokenType.FloatPointer ||
                currentType == TokenType.DoublePointer ||
                currentType == TokenType.CharPointer)
                return ParseIdentifier(currentPosition + 1, errors);

            return GetMinErrors(
                ParseIdentifier(currentPosition, CreateErrorListWithTypePtr(currentPosition, ErrorType.PUSH, errors, _tokens[currentPosition].Line)),
                ParseIdentifier(currentPosition + 1, CreateErrorListWithTypePtr(currentPosition, ErrorType.REPLACE, errors, _tokens[currentPosition].Line)),
                ParseStart(currentPosition + 1, CreateErrorListWithTypePtr(currentPosition, ErrorType.DELETE, errors, _tokens[currentPosition].Line))
            );
        }

        private List<ErrorToken> ParseIdentifier(int currentPosition, List<ErrorToken> errors)
        {
            if (currentPosition >= _tokens.Count)
                return errors;

            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (_tokens[currentPosition].TypeCode == TokenType.Space)
                return ParseIdentifier(currentPosition + 1, errors);

            if (_tokens[currentPosition].TypeCode != TokenType.Identifier)
            {
                return GetMinErrors(
                    ParseEqual(currentPosition, CreateErrorList(currentPosition, TokenType.Identifier, ErrorType.PUSH, errors, _tokens[currentPosition].Line)),
                    ParseEqual(currentPosition + 1, CreateErrorList(currentPosition, TokenType.Identifier, ErrorType.REPLACE, errors, _tokens[currentPosition].Line)),
                    ParseIdentifier(currentPosition + 1, CreateErrorList(currentPosition, TokenType.Identifier, ErrorType.DELETE, errors, _tokens[currentPosition].Line))
                );
            }
            
            return ParseEqual(currentPosition + 1, errors);
        }

        private List<ErrorToken> ParseEqual(int currentPosition, List<ErrorToken> errors)
        {
            if (currentPosition >= _tokens.Count)
                return errors;

            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (_tokens[currentPosition].TypeCode == TokenType.Space)
                return ParseEqual(currentPosition + 1, errors);

            if (_tokens[currentPosition].TypeCode != TokenType.Equal)
            {
                return GetMinErrors(
                    ParseNew(currentPosition, CreateErrorList(currentPosition, TokenType.Equal, ErrorType.PUSH, errors, _tokens[currentPosition].Line)),
                    ParseNew(currentPosition + 1, CreateErrorList(currentPosition, TokenType.Equal, ErrorType.REPLACE, errors, _tokens[currentPosition].Line)),
                    ParseEqual(currentPosition + 1, CreateErrorList(currentPosition, TokenType.Equal, ErrorType.DELETE, errors, _tokens[currentPosition].Line))
                );
            }

            return ParseNew(currentPosition + 1, errors);
        }

        private List<ErrorToken> ParseNew(int currentPosition, List<ErrorToken> errors)
        {
            if (currentPosition >= _tokens.Count)
                return errors;

            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (_tokens[currentPosition].TypeCode == TokenType.Space)
                return ParseNew(currentPosition + 1, errors);

            if (_tokens[currentPosition].TypeCode != TokenType.New)
            {
                return GetMinErrors(
                    ParseSpaceAfterNew(currentPosition, CreateErrorList(currentPosition, TokenType.New, ErrorType.PUSH, errors, _tokens[currentPosition].Line)),
                    ParseSpaceAfterNew(currentPosition + 1, CreateErrorList(currentPosition, TokenType.New, ErrorType.REPLACE, errors, _tokens[currentPosition].Line)),
                    ParseNew(currentPosition + 1, CreateErrorList(currentPosition, TokenType.New, ErrorType.DELETE, errors, _tokens[currentPosition].Line))
                );
            }

            return ParseSpaceAfterNew(currentPosition + 1, errors);
        }

        private List<ErrorToken> ParseSpaceAfterNew(int currentPosition, List<ErrorToken> errors)
        {
            if (currentPosition >= _tokens.Count)
                return errors;

            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (_tokens[currentPosition].TypeCode != TokenType.Space)
            {
                return GetMinErrors(
                    ParseType(currentPosition, CreateErrorList(currentPosition, TokenType.Space, ErrorType.PUSH, errors, _tokens[currentPosition].Line)),
                    ParseType(currentPosition + 1, CreateErrorList(currentPosition, TokenType.Space, ErrorType.REPLACE, errors, _tokens[currentPosition].Line)),
                    ParseSpaceAfterNew(currentPosition + 1, CreateErrorList(currentPosition, TokenType.Space, ErrorType.DELETE, errors, _tokens[currentPosition].Line))
                );
            }

            return ParseType(currentPosition + 1, errors);
        }

        private List<ErrorToken> ParseType(int currentPosition, List<ErrorToken> errors)
        {
            if (currentPosition >= _tokens.Count)
                return errors;

            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (_tokens[currentPosition].TypeCode == TokenType.Space)
                return ParseType(currentPosition + 1, errors);

            var currentType = _tokens[currentPosition].TypeCode;

            if (currentType == TokenType.Int ||
                currentType == TokenType.Float ||
                currentType == TokenType.Double ||
                currentType == TokenType.Char)
                return ParseLeftBracket(currentPosition + 1, errors);

            return GetMinErrors(
                ParseLeftBracket(currentPosition, CreateErrorListWithType(currentPosition, ErrorType.PUSH, errors, _tokens[currentPosition].Line)),
                ParseLeftBracket(currentPosition + 1, CreateErrorListWithType(currentPosition, ErrorType.REPLACE, errors, _tokens[currentPosition].Line)),
                ParseType(currentPosition + 1, CreateErrorListWithType(currentPosition, ErrorType.DELETE, errors, _tokens[currentPosition].Line))
            );
        }

        private List<ErrorToken> ParseLeftBracket(int currentPosition, List<ErrorToken> errors)
        {
            if (currentPosition >= _tokens.Count)
                return errors;

            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (_tokens[currentPosition].TypeCode == TokenType.Space)
                return ParseLeftBracket(currentPosition + 1, errors);

            if (currentPosition + 1 < _tokens.Count &&
                _tokens[currentPosition].TypeCode == TokenType.LeftBracket &&
                _tokens[currentPosition + 1].TypeCode == TokenType.Semicolon)
            {
                errors.Add(new ErrorToken(
                    _tokens[currentPosition].Line,
                    currentPosition,
                    "Вставить лексему: ')'",
                    ErrorType.PUSH)
                );

                return ParseSemicolon(currentPosition + 1, errors);
            }

            if (_tokens[currentPosition].TypeCode != TokenType.LeftBracket)
            {
                return GetMinErrors(
                    ParseRightBracket(currentPosition, CreateErrorList(currentPosition, TokenType.LeftBracket, ErrorType.PUSH, errors, _tokens[currentPosition].Line)),
                    ParseRightBracket(currentPosition + 1, CreateErrorList(currentPosition, TokenType.LeftBracket, ErrorType.REPLACE, errors, _tokens[currentPosition].Line)),
                    ParseLeftBracket(currentPosition + 1, CreateErrorList(currentPosition, TokenType.LeftBracket, ErrorType.DELETE, errors, _tokens[currentPosition].Line))
                );
            }

            return ParseRightBracket(currentPosition + 1, errors);
        }

        private List<ErrorToken> ParseRightBracket(int currentPosition, List<ErrorToken> errors)
        {
            if (currentPosition >= _tokens.Count)
                return errors;

            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (_tokens[currentPosition].TypeCode == TokenType.Space)
                return ParseRightBracket(currentPosition + 1, errors);

            if (_tokens[currentPosition].TypeCode == TokenType.Semicolon)
            {
                errors.Add(new ErrorToken(
                    _tokens[currentPosition].Line,
                    currentPosition,
                    "Вставить лексему: ')'",
                    ErrorType.PUSH
                ));
                return ParseSemicolon(currentPosition, errors);
            }

                if (_tokens[currentPosition].TypeCode != TokenType.RightBracket)
            {
                return GetMinErrors(
                    ParseSemicolon(currentPosition, CreateErrorList(currentPosition, TokenType.RightBracket, ErrorType.PUSH, errors, _tokens[currentPosition].Line)),
                    ParseSemicolon(currentPosition + 1, CreateErrorList(currentPosition, TokenType.RightBracket, ErrorType.REPLACE, errors, _tokens[currentPosition].Line)),
                    ParseRightBracket(currentPosition + 1, CreateErrorList(currentPosition, TokenType.RightBracket, ErrorType.DELETE, errors, _tokens[currentPosition].Line))
                );
            }

            return ParseSemicolon(currentPosition + 1, errors);
        }

        private List<ErrorToken> ParseSemicolon(int currentPosition, List<ErrorToken> errors)
        {
            currentPosition = SkipInvalidTokens(currentPosition, errors);

            if (currentPosition >= _tokens.Count)
            {
                errors.Add(new ErrorToken(
                    _tokens.Last().Line,
                    _tokens.Last().StartIdx,
                    CreateErrorMessage(TokenType.Semicolon, ErrorType.PUSH, currentPosition),
                    ErrorType.PUSH
                ));
                return errors;
            }

            if (_tokens[currentPosition].TypeCode == TokenType.Space)
                return ParseSemicolon(currentPosition + 1, errors);

            if (_tokens[currentPosition].TypeCode != TokenType.Semicolon)
            {
                errors.Add(new ErrorToken(
                    _tokens[currentPosition].Line,
                    currentPosition,
                    CreateErrorMessage(TokenType.Semicolon, ErrorType.PUSH, currentPosition),
                    ErrorType.PUSH
                ));
            }

            return errors;
        }

        private int SkipInvalidTokens(int currentPosition, List<ErrorToken> errors)
        {
            while (currentPosition < _tokens.Count && _tokens[currentPosition].TypeCode == TokenType.Invalid)
            {
                errors.Add(new ErrorToken(
                    _tokens[currentPosition].Line,
                    currentPosition,
                    $"Удалить недопустимый символ: {_tokens[currentPosition].Terminal}",
                    ErrorType.DELETE
                ));

                currentPosition++;
            }

            return currentPosition;
        }

        private static List<ErrorToken> GetMinErrors(List<ErrorToken> push, List<ErrorToken> replace, List<ErrorToken> delete)
        {
            if (push.Count < replace.Count && push.Count < delete.Count)
            {
                return push;
            }

            return replace.Count < delete.Count ? replace : delete;
        }

        private List<ErrorToken> CreateErrorList(int currentPosition, TokenType type, ErrorType errorType, List<ErrorToken> currentErrors, int currentLine)
        {
            var newErrors = new List<ErrorToken>(currentErrors);

            newErrors.Add(new ErrorToken(
                currentLine,
                currentPosition,
                CreateErrorMessage(type, errorType, currentPosition),
                errorType
            ));

            return newErrors;
        }

        private List<ErrorToken> CreateErrorListWithType(int currentPosition, ErrorType errorType, List<ErrorToken> currentErrors, int currentLine)
        {
            var newErrors = new List<ErrorToken>(currentErrors);

            newErrors.Add(new ErrorToken(
                currentLine,
                currentPosition,
                CreateTypeErrorMessage(errorType, currentPosition),
                errorType
            ));

            return newErrors;
        }

        private List<ErrorToken> CreateErrorListWithTypePtr(int currentPosition, ErrorType errorType, List<ErrorToken> currentErrors, int currentLine)
        {
            var newErrors = new List<ErrorToken>(currentErrors);

            newErrors.Add(new ErrorToken(
                currentLine,
                currentPosition,
                CreateTypePtrErrorMessage(errorType, currentPosition),
                errorType
            ));

            return newErrors;
        }

        private string CreateErrorMessage(TokenType type, ErrorType errorType, int currentPosition)
        {
            string lexeme = type == TokenType.Identifier
                ? type.ToString()
                : Lexeme.GetLexeme(type);

            return errorType switch
            {
                ErrorType.PUSH => $"Вставить лексему: '{lexeme}'",
                ErrorType.REPLACE => $"Заменить лексему '{_tokens[currentPosition].Terminal}' на лексему '{lexeme}'",
                ErrorType.DELETE => $"Удалить недопустимый символ '{_tokens[currentPosition].Terminal}'",
                _ => string.Empty
            };
        }

        private string CreateTypeErrorMessage(ErrorType errorType, int currentPosition)
        {
            return errorType switch
            {
                ErrorType.PUSH => "Вставить лексему: 'int', 'float', 'double' или 'char'",
                ErrorType.REPLACE => $"Заменить лексему '{_tokens[currentPosition].Terminal}' на лексему 'int', 'float', 'double' или 'char'",
                ErrorType.DELETE => $"Удалить недопустимый символ '{_tokens[currentPosition].Terminal}'",
                _ => string.Empty
            };
        }
        private string CreateTypePtrErrorMessage(ErrorType errorType, int currentPosition)
        {
            return errorType switch
            {
                ErrorType.PUSH => "Вставить лексему: 'int*', 'float*', 'double*' или 'char*'",
                ErrorType.REPLACE => $"Заменить лексему '{_tokens[currentPosition].Terminal}' на лексему 'int*', 'float*', 'double*' или 'char*'",
                ErrorType.DELETE => $"Удалить недопустимый символ '{_tokens[currentPosition].Terminal}'",
                _ => string.Empty
            };
        }
    }
}
