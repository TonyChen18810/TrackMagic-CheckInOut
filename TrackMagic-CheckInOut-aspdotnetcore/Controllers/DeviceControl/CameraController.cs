using System.ComponentModel;
using System.Threading;
using OpenCvSharp;

namespace TrackMagic_CheckInOut_aspdotnetcore
{
    public class CameraController
    {
        private BackgroundWorker CameraWorker;
        private Mat CurrentImage;
        public delegate void ImageUpdateDelegate(Mat image);
        public ImageUpdateDelegate OnImageUpdate;

        public Mat Snapshot { get { return CurrentImage; } }

        public CameraController(string deviceName = null)
        {
            CameraWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            CameraWorker.DoWork += DoReadCamera;
            CameraWorker.ProgressChanged += ProgressChanged;
            CameraWorker.RunWorkerCompleted += RunWorkerCompleted;
            CameraWorker.RunWorkerAsync();
        }

        private void DoReadCamera(object sender, DoWorkEventArgs e)
        {
            VideoCapture capture = new VideoCapture(0);

            if (capture != null)
            {
                Mat image = new Mat();
                while (CameraWorker != null && !CameraWorker.CancellationPending)
                {
                    capture.Read(image);
                    if (image.Empty())
                        break;

                    CameraWorker.ReportProgress(0, image);
                    Thread.Sleep(10);
                }
            }
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Mat image = e.UserState as Mat;
            if (image == null)
                return;

            CurrentImage = image;

            OnImageUpdate?.Invoke(CurrentImage);
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CameraWorker.Dispose();
            CameraWorker = null;
        }
    }
}
