using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BachelorThesis.Models;
using BachelorThesis.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BachelorThesis.Services
{
    public class MainService : IDataStore<Item>
    {
        List<Item> items;

        public MainService()
        {
        }

        private async Task DoGetRequest(object obj)
        {
            URLHttpParams httpParams = (URLHttpParams)obj;
            string url = httpParams.URL;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(client.BaseAddress);
                response.EnsureSuccessStatusCode(); // выброс исключения, если произошла ошибка

                // десериализация ответа в формате json
                var content = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(content);

                MethodInfo method = typeof(JsonConvert).GetMethods()
                                                .Where(m => m.Name == "DeserializeObject")
                                                .Select(m => new {
                                                                    Method = m,
                                                                    Params = m.GetParameters(),
                                                                    Args = m.GetGenericArguments()
                                                                 })
                                                .Where(x => x.Params.Length == 1 &&
                                                            x.Args.Length == 1)
                                                .Select(x => x.Method)
                                                .First();
                Type t = Type.GetType("BachelorThesis.Models." + httpParams.Type);
                t = typeof(ItemRecords<>).MakeGenericType(t);

                MethodInfo generic = method.MakeGenericMethod(t);
                var test = generic.Invoke(null, new[] { o.ToString() });

                FieldInfo property = test.GetType().GetFields().First();
                IEnumerable result = (IEnumerable) property.GetValue(test);
                items = result.Cast<Item>().ToList();
             
                //var itemRecords = JsonConvert.DeserializeObject<ItemRecords<Item>>(o.ToString());
                //items = itemRecords.records;
            }
            catch (Exception ex)
            { Debug.WriteLine(ex); }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false, object obj = null)
        {
            await DoGetRequest(obj);
            return await Task.FromResult(items);
        }
    }
}