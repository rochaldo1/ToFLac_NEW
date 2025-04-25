using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ToFLac_NEW.Model.Lexer;
using ToFLac_NEW.Model.Parser;
using ToFLac_NEW.ViewModel.Commands;

namespace ToFLac_NEW.ViewModel
{
    public class MainVM : BaseVM
    {
        private string _code = string.Empty;
        private string _indexesNumbers = "1\n";
        private int _numbersCount = 1;
        private Lexer _lexer = new();
        private Parser _parser = new();
        private ObservableCollection<Token> _lexemesTokens = new();
        private ObservableCollection<ErrorToken> _errors = new();

        public ObservableCollection<Token> LexemesTokens
        {
            get => _lexemesTokens;
            set => Set(ref _lexemesTokens, value);
        }

        public ObservableCollection<ErrorToken> Errors
        {
            get => _errors;
            set => Set(ref _errors, value);
        }

        public string Code
        {
            get => _code;
            set
            {
                Set(ref _code, value);
                UpdateIndexesNumbers(value);
            }
        }

        public string IndexesNumbers
        {
            get => _indexesNumbers;
            set => Set(ref _indexesNumbers, value);
        }

        private void UpdateIndexesNumbers(string value)
        {
            int numbersSplits = value.Split('\n').Length;
            if (_numbersCount < numbersSplits)
            {
                _numbersCount = numbersSplits;
                IndexesNumbers = IndexesNumbers.Split('\n')[0] + "\n";
                for (int i = 2; i < numbersSplits + 1; i++)
                {
                    IndexesNumbers += $"{i}\n";
                }
            }

            if (_numbersCount > numbersSplits)
            {
                _numbersCount -= 1;
                IndexesNumbers = IndexesNumbers.Split('\n')[0] + "\n";
                for (int i = 2; i < numbersSplits + 1; i++)
                {
                    IndexesNumbers += $"{i}\n";
                }
            }
        }

        public Command ClearCommand => Command.Create(Clear);
        public Command StartCommand => Command.Create(Start);

        public void Clear()
        {
            Code = string.Empty;
            LexemesTokens = new ObservableCollection<Token>();
            Errors = new ObservableCollection<ErrorToken>();
        }

        public void Start()
        {
            string text = Code.Replace("\t", "").Replace("\r", "");
            text = Regex.Replace(text, @" {1,}", " ");

            List<Token> tokens = _lexer.GetLexemes(text);
            LexemesTokens = new ObservableCollection<Token>(_lexer.GetLexemes(text));

            Errors = new ObservableCollection<ErrorToken>(_parser.Parse(tokens));
        }
    }
}
