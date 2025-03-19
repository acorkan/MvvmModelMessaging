using CommunityToolkit.Mvvm.Messaging;
using MileHighWpf.MvvmModelMessagingDemo.Messages;
using MileHighWpf.MvvmModelMessagingDemo.Models;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    class ViewModel2 : DemoViewModelBase
    {
        public ViewModel2(Model2 model)
        {
            _imagingModel = model;
        }

        /// <summary>
        /// Override this to customize the message type, otherwise GenericPropertyMessage will be used.
        /// Be sure to use the same message type that the model will be using!
        /// </summary>
        protected override void RegisterMessage()
        {
            Messenger.Register<ViewModel2, AnotherModelDependentMessage>(this, (recipient, message) =>
            {
                OnModelUpdated(message); // Update the Name property with message content
            });
        }
    }
}
