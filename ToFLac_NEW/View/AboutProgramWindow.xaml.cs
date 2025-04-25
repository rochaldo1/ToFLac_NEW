using System.Windows;

namespace ToFLac_NEW.View
{
    /// <summary>
    /// Логика взаимодействия для AboutProgramWindow.xaml
    /// </summary>
    public partial class AboutProgramWindow : Window
    {
        public AboutProgramWindow()
        {
            InitializeComponent();
            Text.Text =
                "Курсовая работ по дисциплине 'Теория формальных языков и компиляторов'\n" +
                "Тема: Оператор new для создания объекта с инициализацией (вызов конструктора) на языке C++\n" +
                "Выполнил:\n" +
                "Студент:\n" +
                "Хусаинов Артур Мансурович\n" +
                "Группа: АВТ-213\n";
        }
    }
}
