using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTrackMagic.Models;
using System.IO.Ports;
using AllNet;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using System.Runtime.InteropServices;
using System.Diagnostics;
//using AllNet.Utils;
//using System.Web.
using System.IO;
using WebTrackMagic.DeviceControllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace TrackMagic_CheckInOut_aspdotnetcore.Pages
{
    public class CheckInAndOutModel : PageModel
    {

        private readonly IWebHostEnvironment _env;
        public readonly IVendorRepository vendorRepository;
        public readonly IItemRepository itemRepository;
        public readonly IZoneRepository zoneRepository;
        private readonly IItemTrxRepository itemTrxRepository;

        public ItemTrx ItemTrx { get; private set; } // acutal data model

        [BindProperty] public IFormFile Photo { get; set; }

        private ScaleController scaleController;
        private CameraController cameraController;

        public CheckInAndOutModel(IWebHostEnvironment env, IItemTrxRepository repository, IVendorRepository vendorRepo, IItemRepository itemRepo, IZoneRepository zoneRepo)
        {
            _env = env;

            this.itemTrxRepository = repository;

            this.vendorRepository = vendorRepo;
            this.itemRepository = itemRepo;
            this.zoneRepository = zoneRepo;
        }

        public void OnGet()
        {
            //////FixME:
            //ItemTrx = new ItemTrx()
            //{
            //    staffID = string.Empty,
            //    ItemTrxType = (eItemTrxType)0,
            //    vendorID = string.Empty,
            //    itemID = string.Empty,
            //    zoneID = string.Empty,
            //    expDays = 0,
            //    photoUrl = "noimage.jpg",
            //    qty = 0,
            //    qtyUnit = (eUnitType)0,
            //    timestamp = string.Empty,
            //    upc = "<awaiting>",
            //    videoUrl = string.Empty,
            //    weight = 0,
            //    weightUnit = (eUnitType)0,
            //};


            ////FixME:

            ItemTrx = new ItemTrx()
            {
                vendorID = string.Empty,
                itemID = "0",
                expDays = 125,
                ItemTrxType = (eItemTrxType)1,
                photoUrl = "",
                qty = 34,
                qtyUnit = (eUnitType)3,
                staffID = "2",
                timestamp = "",
                upc = "<awaiting>",
                videoUrl = string.Empty,
                weight = 0,
                weightUnit = (eUnitType)0,
                zoneID = "0"
            };

            //DoDeviceSetup();

            //return ItemTrx;
        }

        public void OnPost(ItemTrx itemTrx)
        {

            if (Photo != null)
            {
                itemTrxRepository.UpdateTrx(itemTrx.itemID, itemTrx);
            }


        }

        public void OnPostUpdatePhoto(ItemTrx itemTrx)
        {
            itemTrxRepository.UpdateTrx(ItemTrx.itemID, itemTrx);
            // return ViewComponent();               

        }

        public ActionResult OnPostGetBarcodesFromPic(string data)
        {
            // to do : return something
            //var path = env.WebRootFileProvider.GetFileInfo("images/TrackMagic-ItemNbr-Seq.jpg")?.PhysicalPath;
            //string fullfilepath = Path.Combine(Directory.GetCurrentDirectory(),
            //  "wwwroot", "images", "TrackMagic-ItemNbr-Seq.png");
            string fullfilepath = "TrackMagic-ItemNbr-Seq.jpg"; //linux

            BarCodeReaderController ctl = new BarCodeReaderController(fullfilepath);
            string result = ctl.ReadFromJpg();
            return Content("bar codes scanned = " + result);
        }

        private void DoDeviceSetup()
        {
            Console.WriteLine("*** About to set up devices.");

            ListComs();

            //scale
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                scaleController = new ScaleController("/dev/ttyUSB0"); //Linux -Raspberry PI  FIXME:  soft code somehow
                scaleController.Reader.OnReadWeight = ShowDeviceWeight;
            }
            else
            {
                scaleController = new ScaleController("/dev/tty.usbserial-A95WOU4H");  //MBP
            }

            string filePath = SnapAndSaveLocally();
        }

        void ShowDeviceWeight(Weight weightObj)
        {
            Console.WriteLine("should be writing to console, ");
        }

        void ListComs()
        {
            string[] ports = SerialPort.GetPortNames();
            Console.WriteLine("The following serial ports were found:");

            foreach (string port in ports)
            {
                //Console.Write(port);
                //Console.ReadLine();
                Console.WriteLine(port.ToString());
            }

            // Console.WriteLine(ports[3]);
        }

        string SnapAndSaveLocally()
        {
            //REF
            //string RootPath = "C:\\";
            //string savedFile = "test.avi";
            //string inputPath = Path.Combine(RootPath, "videoinput");

            Process.Start("/bin/bash", "-c \"echo 'SUCCESS - command.exe or bin.bash found.'\"");  //works
            Process proc;
            //string commandToExec = "-c \"fswebcam 'TrackMagic-ItemNbr-Seq.jpg'\"";
            //string commandToExec2 = "-c \"TakePicture.sh '1280x720' 'TrackMagic-ItemNbr-Seq.jpg'\"";
            string commandToExec = "-c \"fswebcam -r 1280x720 'TrackMagic-ItemNbr-Seq.jpg'\"";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine("On Linux");
                //var output = Process.Start("/bin/bash", "-c \"fswebcam 'in-item-seq#.jpg'\"");  //working
                // proc = Process.Start("/bin/bash", commandToExec);  //working
                proc = Process.Start("/bin/bash", commandToExec);  //working
                Console.WriteLine(proc);
            }
            else
            {
                Process.Start("/bin/bash", "-c \"echo 'Unsupported platform - looking for linux'\"");  //works
            }

            return commandToExec;
        }

        // =====
        public IEnumerable<SelectListItem> GetVendorDropdownList()
        {
            var selectList = new List<SelectListItem>();
            IEnumerable<Vendor> elements = vendorRepository.GetAllVendors().Values;

            foreach (Vendor element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.Id,
                    Text = element.CompanyName
                });
            }
            return selectList;
        }

        public IEnumerable<SelectListItem> GetItemDropdownList(string vendorID)
        {
            var selectList = new List<SelectListItem>();
            IEnumerable<Item> elements = itemRepository.GetAllItems().Values;

            foreach (Item element in elements)
            {
                // if (element.vendorID != vendorID || vendorID != string.Empty) continue;

                selectList.Add(new SelectListItem
                {
                    Value = element.itemID,
                    Text = element.title
                });
            }
            return selectList;
        }

        public IEnumerable<SelectListItem> GetZoneDropdownList(string vendorID, string itemID)
        {
            var selectList = new List<SelectListItem>();
            IEnumerable<Zone> elements = zoneRepository.GetAllZones().Values;

            foreach (Zone element in elements)
            {
                // if (element.vendorID != vendorID || vendorID != string.Empty) continue;

                selectList.Add(new SelectListItem
                {
                    Value = element.Id,
                    Text = element.Name
                });
            }
            return selectList;
        }
        //FIXME or post to firebase and use the flow from the MVC app`
        //void SaveAsUniqueFilename()
        //{
        //    Process proc;
        //    string uniqueFilename = string.Empty;

        //    string ts = DateTime.Now.ToShortTimeString();
        //    string tempCommand = "-c \"fswebcam 'image-filename.jpg'\"";
        //    uniqueFilename = "in-itemNbr-" + ts + ".jpg";
        //    tempCommand.Replace("image-filename.jpg", uniqueFilename);

        //    proc = Process.Start("/bin/bash", tempCommand);  //working
        //    Console.WriteLine("after attempted execute =>" + tempCommand);
        //}


        //CommandExecutioner.RunTest();
        //private void TestSaveImage(Mat newImage)
        //{
        //    newImage.SaveImage(@".\test.png");
        //    MyCamera.OnImageUpdate = null;
        //}
        //public ActionResult OnPostTakePicture(string data)
        //{
        //    // to do : return something
        //    //var path = env.WebRootFileProvider.GetFileInfo("images/TrackMagic-ItemNbr-Seq.jpg")?.PhysicalPath;
        //    string path = Path.Combine(Directory.GetCurrentDirectory(),
        //        "wwwroot", "images", "TrackMagic-ItemNbr-Seq.jpg");
        //    BarCodeReader reader = new BarCodeReader(path);

        //    string[] barCodes = reader.ReadCodes();


        //    return Content("bar codes scanned");
        //}

        //public ActionResult OnPostTakePicture(string data)
        //{
        //    // to do : return something
        //    //var path = env.WebRootFileProvider.GetFileInfo("images/TrackMagic-ItemNbr-Seq.jpg")?.PhysicalPath;
        //    string fullpath = Path.Combine(Directory.GetCurrentDirectory(),
        //        "wwwroot", "images", "TrackMagic-ItemNbr-Seq.png");
        //    // create a barcode reader instance
        //    IBarcodeReader reader = new BarcodeReader();
        //    // load a bitmap
        //    //byte[] barcodeBitmap = (Bitmap)Bitmap.FromFile(path).Byte;
        //    Image sourceImage = Image.FromFile(fullpath);
        //    byte[] imgByteArray = AppExtensions.ImageToByteArray(sourceImage);

        //    // detect and decode the barcode inside the bitmap
        //    // RGBLuminanceSource.BitmapFormat format = AppExtensions.GetImageFormat(imgByteArray);
        //    var result = reader.Decode(imgByteArray, sourceImage.Width, sourceImage.Height, RGBLuminanceSource.BitmapFormat.RGB32);
        //    // do something with the result
        //    if (result != null)
        //    {
        //        // txtDecoderType.Text = result.BarcodeFormat.ToString();
        //        // txtDecoderContent.Text = result.Text;
        //    }

        //    return Content("bar codes scanned");
        //}


    }
}

