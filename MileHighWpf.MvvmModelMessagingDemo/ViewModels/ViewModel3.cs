using CommunityToolkit.Mvvm.Messaging;
using MileHighWpf.MvvmModelMessagingDemo.Messages;
using MileHighWpf.MvvmModelMessagingDemo.Models;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    public class ViewModel3 : DemoViewModelBase
    {
        public ViewModel3(Model3 model)
        {
            _imagingModel = model;
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
