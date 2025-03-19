using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileHighWpf.MvvmModelMessagingDemo.Interfaces
{
    public interface IMockImagingModel
    {
        void OpenCameras();

        void CloseCameras();

        void Start();

        void Stop();

        bool IsOpen { get; }

        bool IsRunning { get; }
    }
}
