using MileHighWpf.MvvmModelMessaging;
using MileHighWpf.MvvmModelMessagingDemo.ViewModels;
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
using System.Windows.Threading;

namespace MileHighWpf.MvvmModelMessagingDemo.Views
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // We meeed to record the UI dispatcher since we can not look it up.
            ViewModelBase.UIDispatcher = Dispatcher.CurrentDispatcher;
        }
    }
}