using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using System.Text.Json;
//using AppEnums;

namespace WebTrackMagic.Models
{
    public class Item
    {
        //public string Id { get; set;  }
        public string itemID { get; set; }  //NOTE: virtual field
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string title { get; set; }
        public string vendorID { get; set; }
        public string zoneID { get; set; }   //default zone Id
        public int defaultExpDays { get; set; }
        public eUnitType quantityUnit { get; set; }
        public int maxQty { get; set; }
        public int qty { get; set; }    // derived
        public int minQty { get; set; }
        public eUnitType weightUnit { get; set; }
        public float weight { get; set; }    // derived
        public string upc { get; set; }
        public string photoUrl { get; set; }

        public eItemStatus status { get; set; }


        public override string ToString()
        {
            return JsonSerializer.Serialize<Item>(this);
        }

        public Item() { }
        public Item(string id, string title, eItemStatus status,
            string vendorID, string zoneID, int defaultExpDays,
            int maxQty, int minQty,
            int qty, eUnitType quantityUnit,
            float weight, eUnitType weightUnit,
            string upc, string photoUrl)
        {
            this.itemID = id;
            this.title = title;
            this.status = status;
            this.vendorID = vendorID;
            this.zoneID = zoneID;  // default zone ID
            this.quantityUnit = quantityUnit;
            this.qty = qty;
            this.minQty = minQty;
            this.maxQty = maxQty;
            this.photoUrl = photoUrl;  // master photo?
            this.defaultExpDays = defaultExpDays;
            this.upc = upc;
            this.weight = weight;
            this.weightUnit = weightUnit;
        }

    }
}

