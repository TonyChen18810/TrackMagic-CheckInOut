using FireSharp.Config;
using FireSharp.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using Firebase.Auth;
using Firebase.Database;
using WebTrackMagic.Models;

namespace WebTrackMagic.Services
{
    public abstract class ExtDBService
    {
        // public static string RoutingPath = "Testing/Definitions/";
        public static string RoutingPath = "WebDevelopment/Definitions/";

        public bool IsNew { get; set; }
        public string Mode
        {
            get
            {
                return (IsNew) ? "create" : "update";
            }
        }

        public IFirebaseClient client;
        public IFirebaseConfig fbConfig = new FirebaseConfig()
        {
            BasePath = "https://trackmagic-c2d43.firebaseio.com/",
            //WebAPIKey = "AIzaSyCO5GJjr6AoK6QcsabvnXjJ1ayNplWalsQ",
            AuthSecret = "b0boWvdyRH6i7fLt9lOG09V633FNIqG4MgA8iZQc"
        };
    }
}
