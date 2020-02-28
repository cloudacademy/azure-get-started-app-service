using System;

using LorryModels;

namespace LorryMobile.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Pickup Item { get; set; }
        public ItemDetailViewModel(Pickup item = null)
        {
            Title = item?.FromAddress;
            Item = item;
        }
    }
}
