using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using MileHighWpf.MvvmModelMessagingDemo.Views;
using MileHighWpf.MvvmModelMessagingDemo.Models;
using MileHighWpf.MvvmModelMessagingDemo.ViewModels;
using MileHighWpf.MvvmModelMessagingDemo.Interfaces;
using MileHighWpf.MvvmModelMessaging;

namespace MileHighWpf.MvvmModelMessagingDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ITheApp
    {
        private IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider { get => _serviceProvider; }

        private Window _liveView;

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Model1>();
            services.AddSingleton<Model4>();
            services.AddSingleton<Model3>();
            services.AddSingleton<Model2>();

            // Tab1 V, VM, and M
            services.AddSingleton<ViewModel1>();
            services.AddSingleton<ViewModel2>();
            services.AddSingleton<ViewModel3>();
            services.AddSingleton<ViewModel4>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Request verbose output
            ViewModelBase.TraceMessagesOn = true;

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = new MainWindow();
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.Show();

            _liveView = new SecondWindow();
            _liveView.Show();
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            _liveView?.Close();
        }
    }
}
