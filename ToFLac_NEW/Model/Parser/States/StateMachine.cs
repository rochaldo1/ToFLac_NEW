using ToFLac_NEW.Model.Lexer;

namespace ToFLac_NEW.Model.Parser.States
{
    public class StateMachine
    {
        public List<ErrorToken> Errors { get; } = new();
        private List<Token> _tokens;
        private int _currentTokenIndex;

        public IState CurrentState { get; set; }

        public StateMachine()
        {
            CurrentState = new StartState();
        }

        public void ParseAll(List<Token> tokens)
        {
            _tokens = tokens;
            _currentTokenIndex = 0;
            Errors.Clear();

            while (_currentTokenIndex < _tokens.Count)
            {
                CurrentState.Enter(this);
                _currentTokenIndex++;
            }
        }

        public Token? GetCurrentToken() 
            => (_currentTokenIndex < _tokens.Count) ? _tokens[_currentTokenIndex] : null;

        public void AddError(int line, int index, string message)
            => Errors.Add(new ErrorToken(line, index, message));

        public Token? PeekNextToken()
        => (_currentTokenIndex + 1 < _tokens.Count) ? _tokens[_currentTokenIndex + 1] : null;

        public void MoveNext() => _currentTokenIndex++;

        public int GetCurrentPosition() => _currentTokenIndex;
    }
}
