using System.Reflection.PortableExecutable;
using ToFLac_NEW.Model.Lexer;

namespace ToFLac_NEW.Model.Parser.States
{
    public class StartState : IState
    {
        private static readonly HashSet<TokenType> _expectedTypes = new()
        {
            TokenType.Int,
            TokenType.Float,
            TokenType.Double,
            TokenType.Char
        };

        public void Enter(StateMachine stateMachine)
        {
            var token = stateMachine.GetCurrentToken();

            if (token == null)
            {
                stateMachine.AddError(1, 0, "Неожиданный конец файла: ожидалось объявление типа");
                return;
            }

            if (_expectedTypes.Contains(token.TypeCode))
            {
                stateMachine.CurrentState = new TypeConsumptionState();
                return;
            }

            stateMachine.AddError(token.Line, token.StartIdx, GetErrorMessage(token));
            stateMachine.CurrentState = new RecoveryState();
        }

        public static string GetErrorMessage(Token token)
        {
            return token.TypeCode switch
            {
                TokenType.New => $"Ключевое слово 'new' не может находиться перед типом",
                TokenType.Equal => $"Неожиданный символ '=' перед объявлением типа",
                TokenType.Pointer => $"Неожиданный символ '*' перед объявлением типа",
                TokenType.LeftBracket => $"Неожиданная скобка '(' перед объявлением типа",
                TokenType.RightBracket => $"Неожиданная скобка ')' перед объявлением типа",
                TokenType.Semicolon => $"Неожиданный символ ';' перед объявлением типа",
                TokenType.Invalid => $"Недопустимый символ в объявлении типа: '{token.Terminal}'",
                _ => $"Ожидался тип (int, float, double или char), получено: '{token.Terminal}'"
            };
        }
    }
}
