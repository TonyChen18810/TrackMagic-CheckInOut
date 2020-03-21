using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using Firebase.Auth;
using Firebase.Database;

using WebTrackMagic.Models;

namespace WebTrackMagic.Services
{
    public class ExtDBServiceForZone : ExtDBService, IZoneRepository
    {
        public Action<IEnumerable<Zone>> OnZoneLoaded = delegate { };

        //public string RoutingPath = "Testing/Definitions/";
        public string _modelPath = "Zones/ZoneList/";
        public IWebHostEnvironment WebHostEnvironment { get; }

        public ExtDBServiceForZone(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            Setup();
            GetAllZones();
        }

        //private HashSet<Zone> _ZoneList = new HashSet<Zone>();
        private Dictionary<string, Zone> _ZoneDictionary = new Dictionary<string, Zone>();

        //public ExtDBServiceForZone()
        //{
        //    Setup();
        //    GetAllZones();
        //}

        public void Setup()
        {
            client = new FireSharp.FirebaseClient(fbConfig);
            if (client != null)
            {
                Console.WriteLine("Connection is ON!");
            }
        }

        public int Tally()
        {
            // _ZoneList =  _ZoneList.Distinct<Zone>().ToList();
            return _ZoneDictionary.Count;
        }

        public Dictionary<string, Zone> GetAllZones()
        {
            return GetAllZoneAsync().Result;
        }

        public async Task<Dictionary<string, Zone>> GetAllZoneAsync()
        {
            if (client == null) Setup();

            FirebaseResponse response = await client.GetTaskAsync(RoutingPath + _modelPath);
            string jsonString = response.Body;
            // string jsonKey = response.
            var enumValueSet = JsonConvert.DeserializeObject<IEnumerable<Zone>>(jsonString);
            //enumValueSet = enumValueSet.Where(s => s != null && s.Name != null); // thats all, i want data with Id>10 only

            if (_ZoneDictionary.Count == 0)
            {
                foreach (var Zone in enumValueSet)
                {
                    Console.WriteLine(Zone);
                    //  _ZoneDictionary[response.ResultAs<string>()] = Zone;
                    _ZoneDictionary.TryAdd<string, Zone>(Zone.Id, Zone);
                }
            }
            //OnZoneLoaded(enumValueSet.Values.ToList<Zone>()); //FIXME

            return _ZoneDictionary;
        }

        public Zone GetZoneWith(string Id)
        {
            // return GetAllZoneAsync().Result.FirstOrDefault<Zone>(s => s.Id == Id);
            return _ZoneDictionary[Id];
        }

        // ===================
        public Zone Create(Zone Zone)
        {
            return CreateAsync(Zone).Result;
        }
        public async Task<Zone> CreateAsync(Zone newZone)
        {
            //FIXME: this needs to be
            var _ZoneList = _ZoneDictionary.Values.ToList<Zone>();
            if (_ZoneList.Count == 0) { newZone.Id = "1"; }
            else
            {
                string maxId = (_ZoneList.Aggregate((i1, i2) => Int32.Parse(i1.Id) > Int32.Parse(i2.Id) ? i1 : i2).Id);
                newZone.Id = (Int32.Parse(maxId) + 1).ToString();
            }

            FirebaseResponse response = await client.SetTaskAsync(RoutingPath + _modelPath + newZone.Id + "/", newZone);

            _ZoneDictionary.Add(newZone.Id, newZone);

            return newZone;
        }

        public bool Delete(string id)
        {
            if (id == null) { return false; }

            return DeleteTaskAsync(id).IsCompletedSuccessfully;
        }

        public async Task<bool> DeleteTaskAsync(string Id)
        {
            _ZoneDictionary.Remove(Id);

            FirebaseResponse response = await client.DeleteTaskAsync(RoutingPath + _modelPath + Id + "/");
            //  Zone respZone = response.ResultAs<Zone>();

            return true;
        }

        public bool Update(Zone Zone)
        {

            return UpdateAsync(Zone).IsCompletedSuccessfully;
        }

        public async Task<Zone> UpdateAsync(Zone ZoneChanges)
        {
            _ZoneDictionary[ZoneChanges.Id] = ZoneChanges;

            FirebaseResponse response = await client.UpdateTaskAsync(RoutingPath + _modelPath + ZoneChanges.Id + "/", ZoneChanges);
            Zone result = response.ResultAs<Zone>();

            return result;
        }
    }
}
