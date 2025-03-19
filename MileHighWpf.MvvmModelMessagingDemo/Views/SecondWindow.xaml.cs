using MileHighWpf.MvvmModelMessagingDemo.Interfaces;
using System.Windows;

namespace MileHighWpf.MvvmModelMessagingDemo.Views
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window, ICanClose
    {
        public SecondWindow()
        {
            InitializeComponent();
        }

        public bool IsTimeToClose { get; set; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(IsTimeToClose) { return; }
            e.Cancel = true;
        }
    }
}
