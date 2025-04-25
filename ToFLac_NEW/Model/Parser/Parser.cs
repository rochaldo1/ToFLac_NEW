using ToFLac_NEW.Model.Lexer;

namespace ToFLac_NEW.Model.Parser
{
    public class Parser
    {
        private readonly List<ErrorToken> _errors = new();
        private List<List<Token>> _tokens = new();

        public List<ErrorToken> Parse(List<Token> inputTokens)
        {

        }
    }
}
