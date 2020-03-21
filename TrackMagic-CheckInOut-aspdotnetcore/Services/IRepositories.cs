using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTrackMagic.Models
{
    public interface IStaffRepository
    {
        Dictionary<string, Staff> GetAllStaff();
        Staff GetStaffWith(string Id);
        //Staff Create(Staff staff);
        Staff Create(Staff staff);
        bool Update(Staff staffChanges);
        bool Delete(string id);
        int Tally();
        bool IsNew { get; set; }
        string Mode { get; }
    }

    public interface IVendorRepository
    {
        Dictionary<string, Vendor> GetAllVendors();
        Vendor GetVendorWith(string Id);
        Vendor Create(Vendor vendor);
        bool Update(Vendor vendorChanges);
        bool Delete(string id);
        int Tally();
        bool IsNew { get; set; }
        string Mode { get; }

    }

    public interface IItemRepository
    {
        Dictionary<string, Item> GetAllItems();
        Item GetItemWith(string Id);
        Item Create(Item item);
        bool Update(Item itemChanges);
        bool Delete(string id);
        int Tally();
        bool IsNew { get; set; }
        string Mode { get; }
    }

    public interface IZoneRepository
    {
        Dictionary<string, Zone> GetAllZones();
        Zone GetZoneWith(string Id);
        Zone Create(Zone item);
        bool Update(Zone itemChanges);
        bool Delete(string id);
        int Tally();
        bool IsNew { get; set; }
        string Mode { get; }
    }

    //=====new
    public interface IItemTrxRepository
    {
        Dictionary<string, ItemTrx> GetAllItemTrxs(string itemId);
        ItemTrx GetTrxWithId(string trxId);  //get item with trxId
        //ItemTrx CreateTrx(string itemId, ItemTrx ItemTrx);
        bool UpdateTrx(string itemID, ItemTrx ItemTrxChanges);
        // bool DeleteTrx(string itemID, string trxId);
        int Tally();
        bool IsNew { get; set; }
        string Mode { get; }
    }

}
