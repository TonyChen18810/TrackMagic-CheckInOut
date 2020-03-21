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
    public class ExtDBServiceForVendor : ExtDBService, IVendorRepository
    {
        public Action<IEnumerable<Vendor>> OnVendorLoaded = delegate { };

        //public string RoutingPath = "Testing/Definitions/";
        public string _modelPath = "Vendors/VendorList/";
        public IWebHostEnvironment WebHostEnvironment { get; }

        public ExtDBServiceForVendor(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;

            Setup();
            GetAllVendors();
        }

        //private HashSet<Vendor> _VendorList = new HashSet<Vendor>();
        private Dictionary<string, Vendor> _VendorDictionary = new Dictionary<string, Vendor>();

        //public ExtDBServiceForVendor()
        //{
        //    Setup();
        //    GetAllVendors();
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
            // _VendorList =  _VendorList.Distinct<Vendor>().ToList();
            return _VendorDictionary.Count;
        }

        public Dictionary<string, Vendor> GetAllVendors()
        {
            return GetAllVendorAsync().Result;
        }

        public async Task<Dictionary<string, Vendor>> GetAllVendorAsync()
        {
            if (client == null) Setup();

            FirebaseResponse response = await client.GetTaskAsync(RoutingPath + _modelPath);
            string jsonString = response.Body;
            // string jsonKey = response.
            var enumValueSet = JsonConvert.DeserializeObject<IEnumerable<Vendor>>(jsonString);
            //enumValueSet = enumValueSet.Where(s => s != null && s.Name != null); // thats all, i want data with Id>10 only

            if (_VendorDictionary.Count == 0)
            {
                foreach (var Vendor in enumValueSet)
                {
                    Console.WriteLine(Vendor);
                    //  _VendorDictionary[response.ResultAs<string>()] = Vendor;
                    _VendorDictionary.TryAdd<string, Vendor>(Vendor.Id, Vendor);
                }
            }
            //OnVendorLoaded(enumValueSet.Values.ToList<Vendor>()); //FIXME

            return _VendorDictionary;
        }

        public Vendor GetVendorWith(string Id)
        {
            // return GetAllVendorAsync().Result.FirstOrDefault<Vendor>(s => s.Id == Id);
            return _VendorDictionary[Id];
        }

        // ===================
        public Vendor Create(Vendor Vendor)
        {
            return CreateAsync(Vendor).Result;
        }
        public async Task<Vendor> CreateAsync(Vendor newVendor)
        {
            //FIXME: this needs to be
            var _VendorList = _VendorDictionary.Values.ToList<Vendor>();
            if (_VendorList.Count == 0) { newVendor.Id = "1"; }
            else
            {
                string maxId = (_VendorList.Aggregate((i1, i2) => Int32.Parse(i1.Id) > Int32.Parse(i2.Id) ? i1 : i2).Id);
                newVendor.Id = (Int32.Parse(maxId) + 1).ToString();
            }

            FirebaseResponse response = await client.SetTaskAsync(RoutingPath + _modelPath + newVendor.Id + "/", newVendor);

            _VendorDictionary.Add(newVendor.Id, newVendor);

            return newVendor;
        }

        public bool Delete(string id)
        {
            if (id == null) { return false; }

            return DeleteTaskAsync(id).IsCompletedSuccessfully;
        }

        public async Task<bool> DeleteTaskAsync(string Id)
        {
            _VendorDictionary.Remove(Id);

            FirebaseResponse response = await client.DeleteTaskAsync(RoutingPath + _modelPath + Id + "/");
            //  Vendor respVendor = response.ResultAs<Vendor>();

            return true;
        }

        public bool Update(Vendor Vendor)
        {

            return UpdateAsync(Vendor).IsCompletedSuccessfully;
        }

        public async Task<Vendor> UpdateAsync(Vendor VendorChanges)
        {
            _VendorDictionary[VendorChanges.Id] = VendorChanges;

            FirebaseResponse response = await client.UpdateTaskAsync(RoutingPath + _modelPath + VendorChanges.Id + "/", VendorChanges);
            Vendor result = response.ResultAs<Vendor>();

            return result;
        }
    }
}
