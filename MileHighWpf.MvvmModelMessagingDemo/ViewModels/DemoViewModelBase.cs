using CommunityToolkit.Mvvm.Input;
using MileHighWpf.MvvmModelMessaging;
using MileHighWpf.MvvmModelMessagingDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileHighWpf.MvvmModelMessagingDemo.ViewModels
{
    public partial class DemoViewModelBase : ViewModelBase
    {
        protected IMockImagingModel _imagingModel;

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

    }
}
