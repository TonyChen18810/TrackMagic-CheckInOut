using System.IO;
using System.Linq;
using System.Text.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Firebase.Database;
using WebTrackMagic.Models;

namespace WebTrackMagic.Services
{
    public class ExtDBServiceForStaff : ExtDBService, IStaffRepository
    {
        public Action<IEnumerable<Staff>> OnStaffLoaded = delegate { };

        //public string RoutingPath = "Testing/Definitions/";
        public string _modelPath = "Staff/StaffList/";
        public IWebHostEnvironment WebHostEnvironment { get; }

        public ExtDBServiceForStaff(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;

            Setup();
            GetAllStaff();
        }

        //private HashSet<Staff> _staffList = new HashSet<Staff>();
        private Dictionary<string, Staff> _staffDictionary = new Dictionary<string, Staff>();

        //public ExtDBServiceForStaff()
        //{
        //    //_staffList = new List<Staff> {
        //    //    new Staff() { Id = 1, Name = "Mary", Email = "mary@email.com", Department = eDepartment.IT},
        //    //    new Staff() { Id = 2, Name = "John", Email = "john@email.com", Department = eDepartment.Kitchen },
        //    //    new Staff() { Id = 3, Name = "Sam", Email = "sam@email.com", Department = eDepartment.Wait }
        //    //};
        //    Setup();
        //    GetAllStaff();
        //}

        public void Setup()
        {
            client = new FireSharp.FirebaseClient(fbConfig);
            if (client != null)
            {
                Console.WriteLine("Connection is ON!");
            }
        }

        public Dictionary<string, Staff> GetAllStaff()
        {
            return GetAllStaffAsync().Result;
        }

        public int Tally()
        {
            // _staffList =  _staffList.Distinct<Staff>().ToList();
            return _staffDictionary.Count;
        }

        public async Task<Dictionary<string, Staff>> GetAllStaffAsync()
        {
            if (client == null) Setup();

            FirebaseResponse response = await client.GetTaskAsync(RoutingPath + _modelPath);
            string jsonString = response.Body;
            // string jsonKey = response.
            var enumValueSet = JsonConvert.DeserializeObject<IEnumerable<Staff>>(jsonString);
            //enumValueSet = enumValueSet.Where(s => s != null && s.Name != null); // thats all, i want data with Id>10 only

            if (_staffDictionary.Count == 0)
            {
                foreach (var staff in enumValueSet)
                {
                    Console.WriteLine(staff);
                    //  _staffDictionary[response.ResultAs<string>()] = staff;
                    _staffDictionary.TryAdd<string, Staff>(staff.Id, staff);
                }
            }
            //OnStaffLoaded(enumValueSet.Values.ToList<Staff>()); //FIXME

            return _staffDictionary;
        }

        public Staff GetStaffWith(string Id)
        {
            // return GetAllStaffAsync().Result.FirstOrDefault<Staff>(s => s.Id == Id);
            return _staffDictionary[Id];
        }

        // ===================
        public Staff Create(Staff staff)
        {
            return CreateAsync(staff).Result;
        }
        public async Task<Staff> CreateAsync(Staff newStaff)
        {
            //FIXME: this needs to be
            var _staffList = _staffDictionary.Values.ToList<Staff>();
            string maxId = _staffList.Aggregate((i1, i2) => Int32.Parse(i1.Id) > Int32.Parse(i2.Id) ? i1 : i2).Id;
            newStaff.Id = (Int32.Parse(maxId) + 1).ToString();

            FirebaseResponse response = await client.SetTaskAsync(RoutingPath + _modelPath + newStaff.Id + "/", newStaff);

            _staffDictionary.Add(newStaff.Id, newStaff);

            return newStaff;
        }

        public bool Delete(string id)
        {
            if (id == null) { return false; }

            return DeleteTaskAsync(id).IsCompletedSuccessfully;
        }

        public async Task<bool> DeleteTaskAsync(string Id)
        {
            _staffDictionary.Remove(Id);

            FirebaseResponse response = await client.DeleteTaskAsync(RoutingPath + _modelPath + Id + "/");
            //  Staff respStaff = response.ResultAs<Staff>();

            return true;
        }

        public bool Update(Staff staff)
        {

            return UpdateAsync(staff).IsCompletedSuccessfully;
        }

        public async Task<Staff> UpdateAsync(Staff staffChanges)
        {
            _staffDictionary[staffChanges.Id] = staffChanges;

            FirebaseResponse response = await client.UpdateTaskAsync(RoutingPath + _modelPath + staffChanges.Id + "/", staffChanges);
            Staff result = response.ResultAs<Staff>();

            return result;
        }


    }
}