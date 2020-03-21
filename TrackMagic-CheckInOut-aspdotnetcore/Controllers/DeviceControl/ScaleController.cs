using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Scale;
using System.Web;

namespace WebTrackMagic.DeviceControllers
{
    public class ScaleController// : MonoBehaviour
    {
        public readonly ScaleReader Reader;

        //public Text lbs;
        //public Text kgs;

        public ScaleController(string comName)
        {
            Reader = new ScaleReader();
            Reader.Init(comName);
            //Reader.Init("/dev/tty.usbserial-A95WOU4H");  // 

            Reader.OnReadWeight = PrintWeight;
            Reader.OnReadWeightRaw = PrintWeight;
        }

        private void PrintWeight(Weight weightObj)
        {
            weightObj.PrintInfo();
        }

        private void PrintWeight(List<string> lines)
        {
            string weight = "";
            foreach (string line in lines)
            {
                weight += line;
            }
            Console.WriteLine("ScaleData: " + weight);
        }

        ~ScaleController()
        {
            Reader?.Shutdown();
        }

    }
}
