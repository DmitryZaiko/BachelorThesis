﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using BachelorThesis.Models;
using BachelorThesis.Views;

namespace BachelorThesis.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public INavigation Navigation { get; set; }  
        public ItemType PageType { get; set; }
        
        public ItemsViewModel(URLHttpParams httpParams = null)
        {
            Title = "Mani kursi";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async (args) => {
                httpParams = (URLHttpParams)args ?? httpParams;
                await ExecuteLoadItemsCommand(httpParams);
            });

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        public string Text
        {
            get
            {
                if (Items.Count == 0 || !(Items[0] is Content))
                    return "";

                return ((Content)Items[0]).Text;
            }
        }

        protected async Task ExecuteLoadItemsCommand(object obj)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true, obj);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                if (PageType == ItemType.Content) {
                    OnPropertyChanged("Text");
                    MessagingCenter.Send<ItemsViewModel, object>(this, "ContentLoaded", items);
                }
                MessagingCenter.Send<ItemsViewModel>(this, "ItemsLoaded");
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