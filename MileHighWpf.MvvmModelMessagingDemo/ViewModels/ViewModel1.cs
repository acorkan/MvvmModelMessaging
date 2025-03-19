using MileHighWpf.MvvmModelMessagingDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    public class ViewModel1 : DemoViewModelBase
    {
        public ViewModel1(Model1 model)
        {
            _imagingModel = model;
            // Add a filter based on the model type.
            AllowedMessageSenders.Add(model.GetType().ToString());
        }
    }
}
