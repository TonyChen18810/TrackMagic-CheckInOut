//using System;
//using CameraControl.Devices;
//using CameraControl.Devices.Classes;
//using System.IO;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading;
//using System.Threading.Tasks;
////using System.Web.Http;

//namespace WebTrackMagic.Controllers
//{
//    public class USBCameraController
//    {

//        public string FolderForPhotos = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "DSLR");

//        private TaskCompletionSource<string> TaskCompletionSource = null;
//        private CameraDeviceManager DeviceManager = null;

//        // GET api/photo
//        public async Task<HttpResponseMessage> Get(CancellationToken token)
//        {
//            TaskCompletionSource = new TaskCompletionSource<string>();

//            DeviceManager = new CameraDeviceManager();
//            DeviceManager.DisableNativeDrivers = true;
//            DeviceManager.StartInNewThread = false;
//            DeviceManager.PhotoCaptured += DeviceManager_PhotoCaptured;

//            DeviceManager.ConnectToCamera();
//            DeviceManager.SelectedCameraDevice.CapturePhoto();

//            token.ThrowIfCancellationRequested();
//            var fileName = await TaskCompletionSource.Task.ConfigureAwait(continueOnCapturedContext: false);

//            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

//            var stream = new FileStream(fileName, FileMode.Open);
//            result.Content = new StreamContent(stream);
//            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
//            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fileName);
//            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
//            result.Content.Headers.ContentLength = stream.Length;
//            return result;
//        }

//        private void DeviceManager_PhotoCaptured(object sender, PhotoCapturedEventArgs eventArgs)
//        {
//            if (eventArgs == null)
//            {
//                TaskCompletionSource.TrySetException(new Exception("eventArgs is empty"));
//                return;
//            }
//            try
//            {
//                string fileName = Path.Combine(FolderForPhotos, Path.GetFileName(eventArgs.FileName));
//                // if file exist try to generate a new filename to prevent file lost. 
//                // This useful when camera is set to record in ram the the all file names are same.
//                if (File.Exists(fileName))
//                    fileName =
//                        StaticHelper.GetUniqueFilename(
//                        Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + "_", 0,
//                        Path.GetExtension(fileName));

//                // check the folder of filename, if not found create it
//                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
//                {
//                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
//                }
//                eventArgs.CameraDevice.TransferFile(eventArgs.Handle, fileName);
//                // the IsBusy may used internally, if file transfer is done should set to false  
//                eventArgs.CameraDevice.IsBusy = false;

//                TaskCompletionSource.TrySetResult(fileName);
//            }
//            catch (Exception exception)
//            {
//                TaskCompletionSource.TrySetException(exception);
//                eventArgs.CameraDevice.IsBusy = false;
//            }
//            finally
//            {
//                DeviceManager.CloseAll();
//            }
//        }
//    }
//}