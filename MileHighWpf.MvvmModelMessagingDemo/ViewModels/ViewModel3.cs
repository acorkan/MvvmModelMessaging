using CommunityToolkit.Mvvm.Input;
using MileHighWpf.MvvmModelMessaging;
using MileHighWpf.MvvmModelMessagingDemo.Interfaces;
using MileHighWpf.MvvmModelMessagingDemo.Messages;
using MileHighWpf.MvvmModelMessagingDemo.Models;
using System.Windows;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    /// <summary>
    /// Use templated base class to implement NewModelDependentMessage messages.
    /// </summary>
    public partial class ViewModel3 : ViewModelBase<NewModelDependentMessage>
    {
        protected IMockImagingModel _imagingModel;

        public string Title { get => _imagingModel.Title; }
        [ModelDependent(nameof(IsOpen))] // When model sends message with "IsOpen" then this is opdated in the view
        [ModelDependentCallCanExecute(nameof(CloseCamerasCommand), nameof(OpenCamerasCommand),
           nameof(StartLiveImagingCommand), nameof(StopLiveImagingCommand))] // If this is ModelDependent then these RelayCommands are updated as well
        public bool IsOpen { get => _imagingModel.IsOpen; }

        [ModelDependent(nameof(IMockImagingModel.IsRunning))]
        [ModelDependentCallCanExecute(nameof(CloseCamerasCommand), nameof(OpenCamerasCommand),
            nameof(StartLiveImagingCommand), nameof(StopLiveImagingCommand))] // If this is ModelDependent then these RelayCommands are updated as well
        public bool IsRunning { get => _imagingModel.IsRunning; }


        [RelayCommand(CanExecute = nameof(CanOpenCamerasExecute))]
        private async Task OpenCameras()
        {
            await Task.Run(() =>
            {
                _imagingModel.OpenCameras();
            });
        }
        private bool CanOpenCamerasExecute()
        {
            return !_imagingModel.IsOpen;
        }


        [RelayCommand(CanExecute = nameof(CanCloseCamerasExecute))]
        private void CloseCameras()
        {
            _imagingModel.CloseCameras();
        }
        private bool CanCloseCamerasExecute()
        {
            return !_imagingModel.IsRunning && _imagingModel.IsOpen;
        }

        [RelayCommand(CanExecute = nameof(CanStartLiveImagingExecute))]
        private void StartLiveImaging()
        {
            _imagingModel.Start();
        }
        private bool CanStartLiveImagingExecute()
        {
            return _imagingModel.IsOpen && !_imagingModel.IsRunning;
        }

        [RelayCommand(CanExecute = nameof(CanStopLiveImagingExecute))]
        private void StopLiveImaging()
        {
            _imagingModel.Stop();
        }
        private bool CanStopLiveImagingExecute()
        {
            return _imagingModel.IsOpen && _imagingModel.IsRunning;
        }

        public ViewModel3() : base() 
        {
            _imagingModel = (IMockImagingModel)(Application.Current as ITheApp).ServiceProvider.GetService(typeof(Model3));
        }
    }
}
