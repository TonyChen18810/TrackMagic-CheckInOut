using System;
using System.Collections.Generic;
using Firebase.Storage;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Hosting;
using WebTrackMagic.Models;

namespace WebTrackMagic
{
    public class PhotoDownloader
    {
        public string _gsUrl = "gs://trackmagic-c2d43.appspot.com/images/staff/smiley-face.jpg";
        public string _actualUrl;
        public string RoutingPath = "Testing/Definitions/Staff/";
        public string _modelPath = "StaffList/";
        public IWebHostEnvironment WebHostEnvironment { get; }
        //  "https://storage.googleapis.com/storage/v1/b/[BUCKET_NAME]/o/[OBJECT_NAME]?alt=media"
        // gs://trackmagic-c2d43.appspot.com/images/staff/smiley-face.jpg
        // accessToken = https://firebasestorage.googleapis.com/v0/b/trackmagic-c2d43.appspot.com/o/images%2Fstaff%2Fsmiley-face.jpg?alt=media&token=45d1df1c-28d6-428d-a769-b1d44fea71cc

        public IFirebaseClient client;
        public IFirebaseConfig fbConfig = new FirebaseConfig()
        {
            // BasePath = "https://trackmagic-c2d43.firebaseio.com/",
            //WebAPIKey = "AIzaSyCO5GJjr6AoK6QcsabvnXjJ1ayNplWalsQ",
            //AuthSecret = "b0boWvdyRH6i7fLt9lOG09V633FNIqG4MgA8iZQc"
            BasePath = "trackmagic-c2d43.appspot.com",
            AuthSecret = "45d1df1c-28d6-428d-a769-b1d44fea71cc"
        };

        //private HashSet<Staff> _staffList = new HashSet<Staff>();
        private Dictionary<string, Staff> _staffDictionary = new Dictionary<string, Staff>();

        public PhotoDownloader(IWebHostEnvironment webHostEnvironment)
        {
            this.WebHostEnvironment = webHostEnvironment;

        }

        //private void DownloadObject(string bucketName, string objectName,
        //string localPath = null)
        //    {
        //        var storage = StorageClient.Create();
        //        localPath = localPath ?? Path.GetFileName(objectName);
        //        using (var outputFile = File.OpenWrite(localPath))
        //        {
        //            storage.DownloadObject(bucketName, objectName, outputFile);
        //        }
        //        Console.WriteLine($"downloaded {objectName} to {localPath}.");
        //    }



    }
}
