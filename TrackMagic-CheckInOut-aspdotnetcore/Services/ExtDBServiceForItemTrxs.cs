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
    public class ExtDBServiceForItemTrxs : ExtDBService, IItemTrxRepository
    {
        public Action<IEnumerable<Item>> OnItemLoaded = delegate { };

        public string _modelPath = "ItemTransactions/";
        public IWebHostEnvironment WebHostEnvironment { get; }

        //private HashSet<Item> _ItemList = new HashSet<Item>();
        private Dictionary<string, ItemTrx> _ItemTrxDictionary = new Dictionary<string, ItemTrx>();

        public ExtDBServiceForItemTrxs(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            Setup();
            // GetAllItems();
            //GetAllItemTrxs();
        }

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
            // _ItemList =  _ItemList.Distinct<Item>().ToList();
            return _ItemTrxDictionary.Count;
        }

        public Dictionary<string, ItemTrx> GetAllItemTrxs(string itemId)
        {
            return GetAllItemTrxsAsync(itemId).Result;
        }

        public async Task<Dictionary<string, ItemTrx>> GetAllItemTrxsAsync(string itemId)
        {
            if (client == null) Setup();

            FirebaseResponse response = await client.GetTaskAsync(RoutingPath + _modelPath + "/" + itemId);
            string jsonString = response.Body;
            var enumValueSet = JsonConvert.DeserializeObject<IEnumerable<ItemTrx>>(jsonString);
            //enumValueSet = enumValueSet.Where(s => s != null && s.Name != null); // thats all, i want data with Id>10 only

            if (_ItemTrxDictionary.Count == 0)
            {
                foreach (var ItemTrx in enumValueSet)
                {
                    Console.WriteLine(ItemTrx);
                    //  _ItemDictionary[response.ResultAs<string>()] = Item;
                    _ItemTrxDictionary.TryAdd<string, ItemTrx>(itemId, ItemTrx);
                }
            }
            //OnItemLoaded(enumValueSet.Values.ToList<Item>()); //FIXME

            return _ItemTrxDictionary;
        }

        //public ItemTrx GetLastTrxForItem(string itemId)
        //{
        //    // return new ItemTrx();
        //}

        // ===================

        public ItemTrx GetTrxWithId(string Id)
        {
            // return GetAllItemAsync().Result.FirstOrDefault<Item>(s => s.itemId == Id);
            return _ItemTrxDictionary[Id];
        }

        public ItemTrx Create(string itemId, ItemTrx ItemTrx)
        {
            return CreateAsync(itemId, ItemTrx).Result;
        }
        public async Task<ItemTrx> CreateAsync(string itemId, ItemTrx newItemTrx)
        {
            //FIXME: this needs to be
            //var _trxList = _ItemTrxDictionary.Values.ToList<ItemTrx>();
            //if (_ItemList.Count == 0) { newItem.itemID = "1"; }
            //else
            //{
            //    string maxId = (_ItemList.Aggregate((i1, i2) => Int32.Parse(i1.itemID) > Int32.Parse(i2.itemID) ? i1 : i2).itemID);
            //    newItem.itemID = (Int32.Parse(maxId) + 1).ToString();
            //}

            FirebaseResponse response = await client.SetTaskAsync(RoutingPath + _modelPath + itemId + "/", DateTime.Now.ToShortDateString());

            _ItemTrxDictionary.Add(itemId, newItemTrx);

            return newItemTrx;
        }

        public bool DeleteTrx(string id)
        {
            if (id == null) { return false; }

            return DeleteTaskAsync(id).IsCompletedSuccessfully;
        }

        public async Task<bool> DeleteTaskAsync(string Id)  //trx ID
        {
            //_ItemTrxDictionary.Remove(Id);

            //FirebaseResponse response = await client.DeleteTaskAsync(RoutingPath + _modelPath + Id + "/");
            ////  Item respItem = response.ResultAs<Item>();

            return false;
        }

        public bool UpdateTrx(string itemId, ItemTrx ItemTrx)
        {
            return UpdateAsync(itemId, ItemTrx).IsCompletedSuccessfully;
        }

        public async Task<ItemTrx> UpdateAsync(string itemId, ItemTrx ItemChanges)
        {
            _ItemTrxDictionary[ItemChanges.timestamp] = ItemChanges;

            FirebaseResponse response = await client.UpdateTaskAsync(RoutingPath + _modelPath + itemId + "/", ItemChanges);
            ItemTrx result = response.ResultAs<ItemTrx>();

            return result;
        }
    }
}