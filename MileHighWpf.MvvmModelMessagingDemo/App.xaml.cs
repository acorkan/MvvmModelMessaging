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

    private static void ConfigureServices(IServiceCollection services)
    {
        // Main V and VM
        services.AddTransient<MainWindow>();
        services.AddSingleton<MainViewModel>();

        // Tab1 V, VM, and M
        services.AddTransient<Tab1UserControl>();
        services.AddSingleton<Tab1ViewModel>();
        services.AddSingleton<ITab1, Tab1Model>();

        // Tab1 V, VM, and M
        services.AddTransient<Tab2UserControl>();
        services.AddSingleton<Tab2ViewModel>();
        services.AddSingleton<ITab2, Tab2Model>();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();

        //ViewModelBase.TraceModelDependentMessages = true;
    }
}

