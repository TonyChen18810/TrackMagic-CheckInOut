using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ContosoCrafts.Website.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Maker { get; set; }
        //  [JsonPropertyNameAttribute("img")] public string Image { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int[] Ratings { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Product>(this);
        }

        public Product() { }
        public Product(string Id, string title, string maker, string url, string image, string description, int[] ratings)
        {
            this.Id = Id;
            this.Title = title;  // default zone ID
            this.Maker = maker;
            this.Url = url;
            this.Image = image;
            this.Description = description;
            this.Ratings = ratings;
        }

        //        [System.Serializable]
        //        public class Item
        //        {
        //            public string itemID;  //NOTE: virtual field
        //            public string title;
        //            public string vendorID;
        //            public string zoneID; //default zone Id
        //            public int defaultExpDays;
        //            public eUnitType quantityUnit;
        //            public int maxQty;
        //            public int qty;  // derived
        //            public int minQty;
        //            public eUnitType weightUnit;
        //            public float weight;  // derived

        //            public string upc;
        //            public string photoUrl;

        //            public eItemStatus status;
        //            //public int qtyChangeTally;
        //            public UICellMetadata uiMeta;

        //            public Item() { }
        //            public Item(string vendorID, string title, string zoneID, int defaultExpDays,
        //                int qty, eUnitType quantityUnit, float weight, eUnitType weightUnit,
        //                string upc, string photoUrl)
        //            {
        //                this.vendorID = vendorID;
        //                this.zoneID = zoneID;  // default zone ID
        //                this.upc = upc;
        //                this.title = title;
        //                this.qty = qty;
        //                this.quantityUnit = quantityUnit;
        //                this.weight = weight;
        //                this.weightUnit = weightUnit;
        //                this.photoUrl = photoUrl;  // master photo?
        //                this.defaultExpDays = defaultExpDays;
        //            }
        //        }

        //        public static ItemsDB.Item ConvertSnapshotToItem(DataSnapshot snapshot)  // from itemMasterCell
        //        {
        //            string snapKey = snapshot.Key.ToString();
        //            // string itemIDVal = snapshot.Child("itemID").ToString();
        //            string _title = snapshot.Child("title").Value.ToString();
        //            string _qty = snapshot.Child("qty").Value.ToString();
        //            string _qtyUnit = snapshot.Child("quantityUnit").Value.ToString();
        //            string _vendorID = snapshot.Child("vendorID").Value.ToString();
        //            string _zoneID = snapshot.Child("zoneID").Value.ToString();
        //            string _upc = snapshot.Child("upc").Value.ToString();
        //            string _photoUrl = snapshot.Child("photoUrl").Value.ToString();
        //            string _maxQty = snapshot.Child("maxQty").Value.ToString();
        //            string _minQty = snapshot.Child("minQty").Value.ToString();

        //            ItemsDB.Item snapItem = new ItemsDB.Item()
        //            {
        //                itemID = snapKey,
        //                title = _title,
        //                qty = Int32.Parse(_qty),
        //                quantityUnit = (eUnitType)Int32.Parse(_qtyUnit),
        //                vendorID = _vendorID,
        //                zoneID = _zoneID,
        //                upc = _upc,
        //                photoUrl = _photoUrl,
        //                maxQty = Int32.Parse(_maxQty),
        //                minQty = Int32.Parse(_minQty)
        //            };

        //            return snapItem;
        //        }
    }
}
