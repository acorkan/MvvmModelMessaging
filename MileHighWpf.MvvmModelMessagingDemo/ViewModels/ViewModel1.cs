using MileHighWpf.MvvmModelMessagingDemo.Interfaces;
using MileHighWpf.MvvmModelMessagingDemo.Models;
using System.Windows;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    public class ViewModel1 : DemoViewModelBase
    {
        public ViewModel1()
        {
            _imagingModel = (IMockImagingModel)(Application.Current as ITheApp).ServiceProvider.GetService(typeof(Model1));
            // Add a filter based on the model type.
            AllowedMessageSenders.Add(_imagingModel.GetType().ToString());
        }
    }
}
