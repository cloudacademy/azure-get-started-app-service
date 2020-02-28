using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LorryModels;

namespace LorryMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Pickup Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Pickup
            {
                Barcode = "",
                FromAddress = "",
                ToAddress = "",
                PickedUp = DateTime.Now,
                Delivered = DateTime.Parse("1899-12-31"),
                SignatureRequired = false
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Item.PickedUp = PickupDate.Date.Add(PickupTime.Time);
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}