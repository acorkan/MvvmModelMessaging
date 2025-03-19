using System;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using MileHighWpf.MvvmModelMessagingDemo.Views;
using MileHighWpf.MvvmModelMessagingDemo.Models;
using MileHighWpf.MvvmModelMessagingDemo.ViewModels;
using MileHighWpf.MvvmModelMessagingDemo.Interfaces;

namespace MileHighWpf.MvvmModelMessagingDemo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    private Window _liveView;

    private static void ConfigureServices(IServiceCollection services)
    {
        // Main V and VM
        services.AddTransient<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();

        // Second V and VM
        services.AddTransient<SecondWindow>();
        services.AddSingleton<SecondWindowViewModel>();

        // Tab1 V, VM, and M
        services.AddSingleton<Tab1ViewModel>();
        services.AddSingleton<ITab1, Tab1Model>();

        // Tab1 V, VM, and M
        services.AddSingleton<Tab2ViewModel>();
        services.AddSingleton<ITab2, Tab2Model>();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        MainWindow.Closed += MainWindow_Closed;
        mainWindow.Show();

        _liveView = ServiceProvider.GetRequiredService<SecondWindow>();
        _liveView?.Show();

        // Request verbose output
        ViewModelBase.TraceModelDependentMessages = true;
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _liveView?.Close();
    }
}

