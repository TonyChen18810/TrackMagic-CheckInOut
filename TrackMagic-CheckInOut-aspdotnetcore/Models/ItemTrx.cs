using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebTrackMagic.Models
{
    public class ItemTrx
    {
        public string itemID; // need to arrive at this format

        public string timestamp; //
        public string vendorID;
        public string staffID;
        public string zoneID;

        public eItemTrxType ItemTrxType;

        [Required]
        // [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public float weight;
        public eUnitType weightUnit;
        public int qty;
        public eUnitType qtyUnit;
        public int expDays;
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string upc;
        [Required]
        public string photoUrl;
        public string videoUrl;
        //[BindProperty]
        //public IFormFile photo { get; set; }


        // public UICellMetadata cellMetadata;

        public ItemTrx() { }  //simple constructor
        public ItemTrx(string timestamp, eItemTrxType type, string zoneID, string staffID,
            float weight, eUnitType weightUnit, int qty, eUnitType qtyUnit, string upc,
            int expDays, string photoUrl, string videoUrl)
        {
            this.timestamp = timestamp;  //trx key

            this.ItemTrxType = type;  // debit, credit, spoil
            this.zoneID = zoneID;
            this.staffID = staffID;
            this.weight = weight;
            this.weightUnit = weightUnit;  //default to item weights unit
            this.qty = qty;
            this.qtyUnit = qtyUnit;  //defaults to items qty unit
            this.expDays = expDays;
            this.photoUrl = photoUrl;
            this.videoUrl = videoUrl;
        }

        public Dictionary<string, System.Object> ToDictionary() //DH
        {
            Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();

            result["timestamp"] = timestamp;
            result["ItemTrxType"] = ItemTrxType;
            result["staffID"] = staffID;
            result["weight"] = weight;
            result["weightUnit"] = weightUnit;
            result["qty"] = qty;
            result["qtyUnit"] = qtyUnit;
            result["expDays"] = expDays;
            result["photoUrl"] = photoUrl;
            result["videoUrl"] = videoUrl;

            return result;
        }
    }
}