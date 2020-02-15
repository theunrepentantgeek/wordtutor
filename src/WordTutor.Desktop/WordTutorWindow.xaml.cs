using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordTutor.Core.Services;

namespace WordTutor.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WordTutorWindow : Window
    {
        private readonly ISpeechService speechService;

        public WordTutorWindow(
            ViewModelToViewValueConverter converter,
            ISpeechService speechService)
        {
            Resources.Add("ViewModelToViewValueConverter", converter);

            InitializeComponent();
            this.speechService = speechService;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            speechService.SayAsync("Hello World.");
        }
    }
}
