// Darryl Hill - AllNet - © 2019
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using TMPro;
//using UnityEngine;
using System.Reflection;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;

namespace AllNet
{
    public struct OptionNameAndIndex
    {
        public string title;
        public int index;
    }

    public static class AppExtensions
    {

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static byte[] ImageToByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }


        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.Bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.Gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.Png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.Tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.Tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.Jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.Jpeg;

            return ImageFormat.Bmp;
        }

        //public static OptionNameAndIndex GetDropdownItemForString(this TMP_Dropdown dropdown, string optionToFind)
        //{
        //    OptionNameAndIndex selection = new OptionNameAndIndex();
        //    int idx = 0;

        //    foreach (TMP_Dropdown.OptionData data in dropdown.options)
        //    {
        //        bool areEqual = String.Equals(optionToFind, data.text, StringComparison.OrdinalIgnoreCase);
        //        if (optionToFind == data.text)
        //        {
        //            selection.title = data.text;
        //            selection.index = idx;
        //            break;
        //        }
        //        idx++;
        //    }
        //    return selection;
        //}


        //public static string GetCurrentTimestamp()
        //{
        //    string ts = DateTime.UtcNow.ToString("yyyyMMddHHmmssff", CultureInfo.InvariantCulture);
        //    int seed = UnityEngine.Random.Range(1, 10);
        //    ts += seed.ToString();

        //    return ts;
        //}

        //public static string GetCurrentTimestampSec()
        //{
        //    string ts = DateTime.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        //    return ts;
        //}

    }
}
