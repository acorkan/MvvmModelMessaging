using CommunityToolkit.Mvvm.Messaging;
using MileHighWpf.MvvmModelMessagingDemo.Interfaces;
using MileHighWpf.MvvmModelMessagingDemo.Messages;
using MileHighWpf.MvvmModelMessagingDemo.Models;
using System.Windows;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    public class ViewModel3 : DemoViewModelBase
    {
        public ViewModel3()
        {
            _imagingModel = (IMockImagingModel)(Application.Current as ITheApp).ServiceProvider.GetService(typeof(Model3));
        }

        /// <summary>
        /// Override this to customize the message type, otherwise GenericPropertyMessage will be used.
        /// Be sure to use the same message type that the model will be using!
        /// </summary>
        protected override void RegisterMessage()
        {
            Messenger.Register<ViewModel3, NewModelDependentMessage>(this, (recipient, message) =>
            {
                OnModelUpdated(message); // Update the Name property with message content
            });
        }
    }
}
