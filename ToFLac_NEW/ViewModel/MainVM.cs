using System.Collections.ObjectModel;
using ToFLac_NEW.Model;
using ToFLac_NEW.ViewModel.Commands;

namespace ToFLac_NEW.ViewModel
{
    public class MainVM : BaseVM
    {
        private string _code = string.Empty;
        private string _outputText = string.Empty;
        private string _indexesNumbers = "1\n";
        private int _numbersCount = 1;
        private ObservableCollection<Error> _errors = new();
        private ObservableCollection<AnalyzerObject> _analyzers = new();

        public ObservableCollection<Error> Errors { get => _errors; set => _errors = value; }
        public ObservableCollection<AnalyzerObject> Analyzers { get => _analyzers; set => _analyzers = value; }

        public string Code
        {
            get => _code;
            set
            {
                Set(ref _code, value);
                UpdateIndexesNumbers(value);
            }
        }

        public string OutputText
        {
            get => _outputText;
            set => Set(ref _outputText, value);
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

        public void Clear()
        {
            Code = string.Empty;
            OutputText = string.Empty;
        }
    }
}
