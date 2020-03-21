using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTrackMagic.Models;

namespace TrackMagic_CheckInOut_aspdotnetcore.Pages
{
    public class ItemTrxListModel : PageModel
    {
        private readonly IItemTrxRepository ItemTrxRepository;
        //public IEnumerable<ItemTrx> ItemTrxs { get; set; }
        public Dictionary<string, ItemTrx> ItemTrxs { get; set; }
        public ItemTrxListModel(IItemTrxRepository repository)
        {
            this.ItemTrxRepository = repository;
        }

        public void OnGet(string id)
        {
            ItemTrxs = ItemTrxRepository.GetAllItemTrxs(id);
        }
    }
}
