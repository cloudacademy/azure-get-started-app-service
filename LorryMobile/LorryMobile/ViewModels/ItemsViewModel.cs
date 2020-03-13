using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using LorryModels;
using LorryMobile.Views;
using LorryMobile.Data;

namespace LorryMobile.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Pickup> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ActivityIndicator Indicator { get; set; }

        public ItemsViewModel()
        {
            Title = "Pickups";
            Items = new ObservableCollection<Pickup>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());


            MessagingCenter.Subscribe<NewItemPage, Pickup>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Pickup;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}