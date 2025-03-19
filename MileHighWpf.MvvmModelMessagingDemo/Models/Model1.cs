using MileHighWpf.MvvmModelMessaging;
using MileHighWpf.MvvmModelMessagingDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileHighWpf.MvvmModelMessagingDemo.Models
{
    public class Model1 : ModelBase, IMockImagingModel
    {
        public string Title => "Mock Imaging Model 1";

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                OnModelChangedSendUpdate(ref _isOpen, value);
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                OnModelChangedSendUpdate(ref _isRunning, value);
            }
        }

        public void CloseCameras()
        {
            if(!IsRunning)
            {
                IsOpen = false;
            }
        }

        public void OpenCameras()
        {
            Thread.Sleep(2000);
            IsOpen = true;
        }

        public void Start()
        {
            if (IsOpen)
            {
                Thread.Sleep(2000);
                IsRunning = true;
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
