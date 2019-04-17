using System;
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
        private Item selectedItem;
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public INavigation Navigation { get; set; }  
        public ItemPageType PageType { get; set; }
        
        public ItemsViewModel(URLHttpParams httpParams = null)
        {
            Title = "Mani kursi";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(httpParams));

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

        //public Item SelectedItem
        //{
        //    get { return selectedItem; }
        //    set
        //    {
        //        if (selectedItem != value)
        //        {
        //            Item item = value;
        //            selectedItem = item;
        //            if (item == null)
        //                return;

        //            ItemPageType nextPageType;
        //            URLHttpParams httpParams;
        //            string title;

        //            switch (PageType)
        //            {
        //                case ItemPageType.Course:
        //                    nextPageType = ItemPageType.Lesson;
        //                    title = "Nodarbības";
        //                    selectedItem = null;
        //                    break;
        //                case ItemPageType.Lesson:
        //                    nextPageType = ItemPageType.Content;
        //                    title = item.Name;
        //                    break;
        //                default:
        //                    nextPageType = ItemPageType.Course;
        //                    title = "Mani Kursi";
        //                    selectedItem = null;
        //                    break;
        //            }

        //            httpParams = new URLHttpParams(nextPageType, item.Id.ToString());
        //            Navigation.PushAsync(new ItemsPage(httpParams, nextPageType) { Title = title });
        //            //SetProperty(ref selectedItem, value);
        //        }
        //    }
        //}

        async Task ExecuteLoadItemsCommand(object obj)
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
                if (PageType == ItemPageType.Content) OnPropertyChanged("Text");
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