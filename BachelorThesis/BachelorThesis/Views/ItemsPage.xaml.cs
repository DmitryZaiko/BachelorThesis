using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BachelorThesis.Models;
using BachelorThesis.Views;
using BachelorThesis.ViewModels;

namespace BachelorThesis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel(new URLHttpParams("Course")){
                                                        PageType = ItemPageType.Course };
        }

        public ItemsPage(URLHttpParams httpParams, ItemPageType pageType)
        {
            InitializeComponent();
            if(pageType == ItemPageType.Content)
            {
                Label label = new Label();
                Content = label;
            }

            BindingContext = viewModel = new ItemsViewModel(httpParams){
                                                   PageType = pageType };
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            ItemPageType nextPageType;
            URLHttpParams httpParams;
            string title;

            switch (viewModel.PageType)
            {
                case ItemPageType.Course:
                    nextPageType = ItemPageType.Lesson;
                    title = "Nodarbības";
                    break;
                case ItemPageType.Lesson:
                    nextPageType = ItemPageType.Content;
                    title = item.Name;
                    break;
                default:
                    nextPageType = ItemPageType.Course;
                    title = "Mani Kursi";
                    break;
            }

            httpParams = new URLHttpParams(nextPageType, item.Id.ToString());
            await Navigation.PushAsync(new ItemsPage(httpParams, nextPageType) { Title = title });

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}