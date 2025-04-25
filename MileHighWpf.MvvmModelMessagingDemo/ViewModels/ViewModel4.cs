﻿using MileHighWpf.MvvmModelMessagingDemo.Interfaces;
using MileHighWpf.MvvmModelMessagingDemo.Models;
using System.Windows;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    /// <summary>
    /// Use default ModelDependentMessage and add the model type name to AllowedMessageSenders to filter other messages.
    /// </summary>
    public class ViewModel4 : DemoViewModelBase
    {
        public ViewModel4()
        {
            _imagingModel = (IMockImagingModel)(Application.Current as ITheApp).ServiceProvider.GetService(typeof(Model4));
            // Add a filter based on the model type.
            AllowedMessageSenders.Add(_imagingModel.GetType().ToString());
        }
    }
}
