using System;
using System.IO;
using Bytescout.BarCodeReader;
using Aspose.BarCode;
using Aspose.BarCode.BarCodeRecognition;
using System.Drawing;

namespace TrackMagic_CheckInOut_aspdotnetcore
{
    public class BarCodeReaderController
    {
        string ImageFile;

        public BarCodeReaderController(string filename)
        {
            ImageFile = filename;
        }

        public string ReadFromJpg()
        {
            // The path to the documents directory.
            // string dataDir = RunExamples.GetDataDir_ManageAndOptimizeBarcodeRecognition();

            // Initialize the BarCodeReader object and Call read method
            // BarCodeReader reader = new BarCodeReader(dataDir + "Barcode2.png", DecodeType.AllSupportedTypes);
            BarCodeReader reader = new BarCodeReader(ImageFile, DecodeType.AllSupportedTypes);

            // To get all possible barcodes
            reader.QualitySettings = QualitySettings.MaxBarCodes;
            string output = string.Empty;
            foreach (BarCodeResult result in reader.ReadBarCodes())
            {
                // Display code text, symbology, detected angle, recognition percentage of the barcode
                Console.WriteLine("Code Text: " + result.CodeText + " Symbology: " + result.CodeType + " Recognition percentage: " + result.Region.Angle);

                // Display x and y coordinates of barcode detected
                Point[] point = result.Region.Points;
                Console.WriteLine("Top left coordinates: X = " + point[0].X + ", Y = " + point[0].Y);
                Console.WriteLine("Bottom left coordinates: X = " + point[1].X + ", Y = " + point[1].Y);
                Console.WriteLine("Bottom right coordinates: X = " + point[2].X + ", Y = " + point[2].Y);
                Console.WriteLine("Top right coordinates: X = " + point[3].X + ", Y = " + point[3].Y);
                output = result.CodeText;
            }

            return output;

        }

        public string[] ReadCodes()
        {
            Console.WriteLine("Reading barcode(s) from image {0}", Path.GetFullPath(ImageFile));

            Reader reader = new Reader();
            reader.RegistrationName = "demo";
            reader.RegistrationKey = "demo";

            // Set barcode type to find
            reader.BarcodeTypesToFind.All = true;
            // We recommend to use specific barcode types to avoid false positives, e.g.:
            // reader.BarcodeTypesToFind.QRCode = true;
            //reader.BarcodeTypesToFind.Code39 = true;

            // Read barcodes
            FoundBarcode[] barcodes = reader.ReadFrom(ImageFile);
            string[] outCodes = new string[10];
            int i = 0;
            foreach (FoundBarcode barcode in barcodes)
            {
                Console.WriteLine("Found barcode with type '{0}' and value '{1}'", barcode.Type, barcode.Value);
                outCodes[i] = barcode.Value;
                i++;
            }

            //  Console.WriteLine("Press any key to exit..");
            // Console.ReadKey();


            return outCodes;
        }
    }

}
