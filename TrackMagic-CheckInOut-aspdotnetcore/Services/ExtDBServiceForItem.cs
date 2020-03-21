
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
    public class ExtDBServiceForItem : ExtDBService, IItemRepository
    {
        public Action<IEnumerable<Item>> OnItemLoaded = delegate { };

        public string _modelPath = "Items/ItemsList/";
        public IWebHostEnvironment WebHostEnvironment { get; }

        public ExtDBServiceForItem(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;

            Setup();
            GetAllItems();
        }

        //private HashSet<Item> _ItemList = new HashSet<Item>();
        private Dictionary<string, Item> _ItemDictionary = new Dictionary<string, Item>();

        //  public ExtDBServiceForItem()
        //{
        //    Setup();
        //    GetAllItems();
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
            // _ItemList =  _ItemList.Distinct<Item>().ToList();
            return _ItemDictionary.Count;
        }

        public Dictionary<string, Item> GetAllItems()
        {
            return GetAllItemsAsync().Result;
        }

        public async Task<Dictionary<string, Item>> GetAllItemsAsync()
        {
            if (client == null) Setup();

            FirebaseResponse response = await client.GetTaskAsync(RoutingPath + _modelPath);
            string jsonString = response.Body;
            var enumValueSet = JsonConvert.DeserializeObject<IEnumerable<Item>>(jsonString);
            //enumValueSet = enumValueSet.Where(s => s != null && s.Name != null); // thats all, i want data with Id>10 only

            if (_ItemDictionary.Count == 0)
            {
                foreach (var Item in enumValueSet)
                {
                    Console.WriteLine(Item);
                    //  _ItemDictionary[response.ResultAs<string>()] = Item;
                    _ItemDictionary.TryAdd<string, Item>(Item.itemID, Item);
                }
            }
            //OnItemLoaded(enumValueSet.Values.ToList<Item>()); //FIXME

            return _ItemDictionary;
        }

        public Item GetItemWith(string Id)
        {
            // return GetAllItemAsync().Result.FirstOrDefault<Item>(s => s.itemId == Id);
            return _ItemDictionary[Id];
        }

        // ===================
        public Item Create(Item Item)
        {
            return CreateAsync(Item).Result;
        }
        public async Task<Item> CreateAsync(Item newItem)
        {
            //FIXME: this needs to be
            var _ItemList = _ItemDictionary.Values.ToList<Item>();
            if (_ItemList.Count == 0) { newItem.itemID = "1"; }
            else
            {
                string maxId = (_ItemList.Aggregate((i1, i2) => Int32.Parse(i1.itemID) > Int32.Parse(i2.itemID) ? i1 : i2).itemID);
                newItem.itemID = (Int32.Parse(maxId) + 1).ToString();
            }

            FirebaseResponse response = await client.SetTaskAsync(RoutingPath + _modelPath + newItem.itemID + "/", newItem);

            _ItemDictionary.Add(newItem.itemID, newItem);

            return newItem;
        }

        public bool Delete(string id)
        {
            if (id == null) { return false; }

            return DeleteTaskAsync(id).IsCompletedSuccessfully;
        }

        public async Task<bool> DeleteTaskAsync(string Id)
        {
            _ItemDictionary.Remove(Id);

            FirebaseResponse response = await client.DeleteTaskAsync(RoutingPath + _modelPath + Id + "/");
            //  Item respItem = response.ResultAs<Item>();

            return true;
        }

        public bool Update(Item Item)
        {

            return UpdateAsync(Item).IsCompletedSuccessfully;
        }

        public async Task<Item> UpdateAsync(Item ItemChanges)
        {
            _ItemDictionary[ItemChanges.itemID] = ItemChanges;

            FirebaseResponse response = await client.UpdateTaskAsync(RoutingPath + _modelPath + ItemChanges.itemID + "/", ItemChanges);
            Item result = response.ResultAs<Item>();

            return result;
        }
    }
}

