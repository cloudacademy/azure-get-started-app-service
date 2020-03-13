using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LorryMobile.Models;
using LorryModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LorryMobile.Services
{
    public class LorryMobileData : IDataStore<Pickup>
    {
        readonly List<Pickup> items;
        private HttpClient client;

        public LorryMobileData()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        public async Task<T> GetReponse<T>(string _apiUrl) where T : class
        {
            List<string> errors = new List<string>();
            try
            {
                var response = await client.GetAsync(_apiUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                    try
                    {

                        var responseObject = JsonConvert.DeserializeObject<T>(jsonResult, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Converters = { new IsoDateTimeConverter() } });
                        return responseObject;
                    }
                    catch (Exception e)
                    {
                        string s = errors.ToString();
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
                return null;
            }
            return null;
        }

        public async Task<T> PostResponse<T>(string _apiUrl, string _json) where T : class
        {
            //var token = App.TokenDb.Get();
            string contentType = "application/json";
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token.access_token);
            try
            {
                var response = await client.PostAsync(_apiUrl, new StringContent(_json, Encoding.UTF8, contentType));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var responseObject = JsonConvert.DeserializeObject<T>(jsonResult);
                        return responseObject;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public async Task<bool> AddItemAsync(Pickup item)
        {
            items.Add(item);
            string apiurl = $"{Constants.ApiURL}pickups/create";
            string json = JsonConvert.SerializeObject(item);
            Pickup pu = await PostResponse<Pickup>(apiurl, json);
            return true;
        }

        public async Task<bool> UpdateItemAsync(Pickup item)
        {
            var oldItem = items.Where((Pickup arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((Pickup arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Pickup> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Pickup>> GetItemsAsync(bool forceRefresh = false)
        {
            List<Pickup> pickups = null;
            string apiurl = $"{Constants.ApiURL}pickups";
            pickups = await GetReponse<List<Pickup>>(apiurl);
            return pickups;
        }
    }
}

