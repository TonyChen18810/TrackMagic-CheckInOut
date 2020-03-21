using System;
using System.Collections.Generic;
using System.IO.Ports;
//using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTrackMagic.DeviceControllers
{
    public class ScaleReader
    {
        private SerialPort ScalePort;

        private List<string> ReadLines;
        private bool SendNextLine;

        public delegate void ReadWeightLines(List<string> lines);
        public ReadWeightLines OnReadWeightRaw;

        public delegate void ReadWeight(Weight weight);
        public ReadWeight OnReadWeight;

        public void Init(string comPort)
        {
            ScalePort = new SerialPort(comPort);

            ScalePort.BaudRate = 9600;
            ScalePort.Parity = Parity.None;
            ScalePort.StopBits = StopBits.One;
            ScalePort.DataBits = 7;
            ScalePort.Handshake = Handshake.None;
            ScalePort.DtrEnable = true;

            ReadLines = new List<string>();

            ScalePort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            ScalePort.Open();
        }

        public void Shutdown()
        {
            ScalePort?.Close();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort senderPort = (SerialPort)sender;
            string lineData = senderPort.ReadExisting();
            //Console.WriteLine(lineData);

            ReadLines.Add(lineData);

            if (SendNextLine == true)
            {
                //	if (!lineData.EndsWith("z") || lineData.EndsWith("b") || lineData.EndsWith("kg"))
                //		return;

                SendWeight();

                SendNextLine = false;
                ReadLines.Clear();
            }
            else if (lineData.ToLower().Contains("net"))
                SendNextLine = true;
        }

        private void SendWeight()
        {
            OnReadWeightRaw?.Invoke(ReadLines);

            string weight = "";
            foreach (string line in ReadLines)
            {
                weight += line;
            }

            int netIndex = weight.IndexOf("Net: ");
            int startIndex = netIndex + 5;
            string netWeight = weight.Substring(startIndex, weight.Length - (startIndex));

            netWeight = netWeight.TrimStart();
            netWeight = netWeight.Replace('\r', ' ');

            Weight weightObj = new Weight();

            if (netWeight.Contains("lb"))
            {
                int lbIndex = netWeight.IndexOf("lb");
                string lb = netWeight.Substring(0, lbIndex);

                if (netWeight.Contains("oz"))
                {
                    int lbInt = int.Parse(lb);

                    int ozStartIndex = lbIndex + 2;
                    int ozFinishIndex = netWeight.IndexOf("oz");
                    string oz = netWeight.Substring(ozStartIndex, ozFinishIndex - ozStartIndex);
                    oz = oz.TrimStart();
                    int ozInt = int.Parse(oz);

                    weightObj.SetPoundsAndOunce(lbInt, ozInt);
                }
                else
                {
                    float lbFloat = float.Parse(lb);
                    weightObj.SetPounds(float.Parse(lb));
                }
            }
            else if (netWeight.Contains("kg"))
            {
                int kgIndex = netWeight.IndexOf("kg");
                string kg = netWeight.Substring(0, kgIndex);
                kg = kg.TrimStart();
                float kgFloat = float.Parse(kg);
                weightObj.SetKg(kgFloat);
            }

            OnReadWeight?.Invoke(weightObj);

            //Console.WriteLine("My Net:" + netWeight);
        }
    }
}
