// Darryl Hill - AllNet - 2019
//using UnityEngine;

namespace WebTrackMagic.Models
{
    public delegate void RecordsTally(int recordCount, bool shouldAppend);

    public enum eZoneType
    {
        Frozen, Produce, Refrigerator, DryStorage, General, Supplies
    }
    public enum eVendorType
    {
        meat, alcohol, speciality, supplies, other
    }
    ////[SerializeField]
    public enum eTaskPriority
    {
        None, Now, Minutes, Hour, Shift, Week
    }

    //[SerializeField]
    public enum eTaskStatus
    {
        None, Assigned, Active, Held, Cancelled, Completed
    }

    //[SerializeField]
    public enum eUnitType
    {
        None, lbs, each, grams, inches, mm, gals, Count
    }

    //[SerializeField]
    public enum eItemZoneQtyStatus
    {
        New, Normal, Low, Spoiled, Count
    }

    //[SerializeField]
    public enum eItemTrxType
    {
        Credit = 0, Debit = 1, Spoil = 2, Count
    }

    //[SerializeField]
    public enum eItemStatus
    {
        normal, low, ordered, expiring, high, hidden
    }

    //[SerializeField]
    public enum eUICategory
    {
        popovers, modals, toasts, snackbars, topbar, navDrawer, errors
    }

    //[SerializeField]
    public enum eDateType
    {
        None, Created, Updated, Deleted
    }


    //[SerializeField]
    public enum eUserAlertCategory
    {
        None, Quantity, BurnRate, Access, Exceptions  // includes vendor access
    }

    //[SerializeField]
    public enum eUserAlertMethod
    {
        Email, Text, Push, None
    }

    public struct SelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

}