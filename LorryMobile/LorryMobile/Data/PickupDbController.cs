using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LorryModels;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using LorryMobile.Models;


namespace LorryMobile.Data
{
    public partial class PickupDbController
    {
        static PickupDbController defaultInstance = new PickupDbController();
        MobileServiceClient client;

        IMobileServiceSyncTable<Pickup> pickupTable;

        const string offlineDbPath = @"localstore.db";

        private PickupDbController()
        {
            this.client = new MobileServiceClient(Constants.ApiURL);

            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<Pickup>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.pickupTable = client.GetSyncTable<Pickup>();
        }

        public static PickupDbController DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }
        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return pickupTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Pickup>; }
        }

        public async Task<ObservableCollection<Pickup>> GetPickupItemsAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await this.SyncAsync();
                }
                IEnumerable<Pickup> items = await pickupTable
                    .Where(pickupItem => pickupItem.Delivered == Constants.Undelivered )
                    .ToEnumerableAsync();

                return new ObservableCollection<Pickup>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine("Invalid sync operation: {0}", new[] { msioe.Message });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Sync error: {0}", new[] { e.Message });
            }
            return null;
        }

        public async Task SaveTaskAsync(Pickup item)
        {
            try
            {
                if (item.Id == -1)
                {
                    await pickupTable.InsertAsync(item);
                }
                else
                {
                    await pickupTable.UpdateAsync(item);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.pickupTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allPickupItems",
                    this.pickupTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}
