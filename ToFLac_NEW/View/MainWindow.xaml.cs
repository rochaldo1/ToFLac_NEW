using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToFLac_NEW.ViewModel;

namespace ToFLac_NEW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SaveFileDialog _saveFileDialog;
        private OpenFileDialog _openFileDialog;
        private string _fileName = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();

            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.DefaultExt = ".txt";
            _saveFileDialog.Filter = "Text documents (.txt)|*.txt";

            _openFileDialog = new OpenFileDialog();
            _openFileDialog.DefaultExt = ".txt";
            _openFileDialog.Filter = "Text documents (.txt)|*.txt";
        }

        private void ButtonCreate_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_fileName) || !string.IsNullOrEmpty(codeBox.Text))
            {
                MessageBoxResult result = MessageBox.Show
                    (
                    "Хотите сохранить файл перед созданием нового файла?",
                    "Подтверждение",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question
                    );

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(sender, e);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            codeBox.Clear();
            _fileName = string.Empty;
        }

        private void ButtonOpen_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_fileName) || !string.IsNullOrEmpty(codeBox.Text))
            {
                MessageBoxResult result = MessageBox.Show
                    (
                    "Хотите сохранить файл перед открытием нового файла?",
                    "Подтверждение",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question
                    );

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(sender, e);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                codeBox.Text = File.ReadAllText(openFileDialog.FileName);
                _fileName = openFileDialog.FileName;
            }
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFile(sender, e);
        }

        private void ButtonSaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileAs(sender, e);
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                SaveFileAs(sender, e);
            }
            else
            {
                File.WriteAllText(_fileName, codeBox.Text);
            }
        }

        private void SaveFileAs(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                DefaultExt = ".txt"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, codeBox.Text);
                _fileName = saveFileDialog.FileName;
            }
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_fileName) || !string.IsNullOrEmpty(codeBox.Text))
            {
                MessageBoxResult result = MessageBox.Show
                    (
                    "У вас есть несохраненные изменения. Хотите сохранить файл перед выходом?",
                    "Подтверждение выхода",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question
                    );

                if (result == MessageBoxResult.Yes)
                {
                    SaveFileAs(sender, e);
                    e.Cancel = false;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == MessageBoxResult.No)
                {
                    e.Cancel = false;
                }
            }
        }

        private void ButtonUndo_OnClick(object sender, RoutedEventArgs e)
        {
            codeBox.Undo();
        }

        private void ButtonRedo_OnClick(object sender, RoutedEventArgs e)
        {
            codeBox.Redo();
        }

        private void ButtonCut_OnClick(object sender, RoutedEventArgs e)
        {
            codeBox.Cut();
        }

        private void ButtonCopy_OnClick(object sender, RoutedEventArgs e)
        {
            codeBox.Copy();
        }

        private void ButtonPaste_OnClick(object sender, RoutedEventArgs e)
        {
            codeBox.Paste();
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            codeBox.Clear();
        }

        private void ButtonSelectAll_OnClick(object sender, RoutedEventArgs e)
        {
            codeBox.SelectAll();
        }

        private void ButtonHelp_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAboutProgram_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlockScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                textBoxScrollViewer.ScrollToVerticalOffset(textBlockScrollViewer.VerticalOffset);
            }
        }

        private void TextBoxScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                textBoxScrollViewer.ScrollToVerticalOffset(textBlockScrollViewer.VerticalOffset);
            }
        }
    }
}