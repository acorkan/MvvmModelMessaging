using MileHighWpf.MvvmModelMessagingDemo.Models;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    class ViewModel4 : DemoViewModelBase
    {
        public ViewModel4(Model2 model)
        {
            _imagingModel = model;
            // Add a filter based on the model type.
            AllowedMessageSenders.Add(model.GetType().ToString());
        }
    }
}
