using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UnityEngine.Events;

namespace WebTrackMagic.DeviceControllers
{
    public class Weight
    {
        // public static event UnityAction<Tuple<float, float>> OnWeightPulled = delegate { };  // lbs and kilograms

        private const float KiloToPoundsRatio = 2.2046226f;
        private const float KiloToOzRatio = 0.0283495f;
        private const int OunceToPoundRatio = 16;

        private float KiloGrams = 0f;

        public void SetKg(float kg)
        {
            KiloGrams = kg;
        }

        public void SetPounds(float lbs)
        {
            KiloGrams = lbs / KiloToPoundsRatio;
        }

        public void SetPoundsAndOunce(int pounds, int ounces)
        {
            float poundKilos = pounds / KiloToPoundsRatio;
            float ozKilos = ounces * KiloToOzRatio;

            KiloGrams = poundKilos + ozKilos;
        }

        public float AsPounds
        {
            get { return (KiloGrams * KiloToPoundsRatio); }
        }

        public float AsKiloGrams
        {
            get { return KiloGrams; }
        }

        public int AsOunces
        {
            get { return (int)(KiloGrams / KiloToOzRatio); }
        }

        public int OuncesRemaining
        {
            get { return (int)(KiloGrams / KiloToOzRatio) % OunceToPoundRatio; }
        }

        public void PrintInfo()
        {
            Console.WriteLine("Lbs: " + AsPounds);
            Console.WriteLine("Ounces: " + AsOunces);
            Console.WriteLine("Kg: " + AsKiloGrams);
            Console.WriteLine("Lbs: " + (int)AsPounds + " Oz: " + OuncesRemaining);

            // OnWeightPulled(new Tuple<float, float>(AsPounds, AsKiloGrams));
        }
    }
}
