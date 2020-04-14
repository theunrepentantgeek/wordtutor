using System.Windows;
using WordTutor.Core.Services;

namespace WordTutor.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WordTutorWindow : Window
    {
        public WordTutorWindow(ViewModelToViewValueConverter converter)
        {
            Resources.Add("ViewModelToViewValueConverter", converter);

            InitializeComponent();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
