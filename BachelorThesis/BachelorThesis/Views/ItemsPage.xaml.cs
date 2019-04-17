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
using System.Diagnostics;
using System.Threading;

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
                                                        PageType = ItemPageType.Course,
                                                        Navigation = this.Navigation};
        }

        public ItemsPage(URLHttpParams httpParams, ItemPageType pageType)
        {
            InitializeComponent();

            viewModel = new ItemsViewModel(httpParams)
                        {
                            PageType = pageType,
                            Navigation = this.Navigation
                        };

            BindingContext = viewModel;

            if (pageType == ItemPageType.Content)
            {
                stackLayout.Children.RemoveAt(0);
                WebView webView = new WebView();
                webView.HorizontalOptions = LayoutOptions.FillAndExpand;
                webView.VerticalOptions = LayoutOptions.FillAndExpand;
                var source = new HtmlWebViewSource();
                source.SetBinding(HtmlWebViewSource.HtmlProperty, "Text");
                webView.Source = source;
                stackLayout.Children.Add(webView);
            }   
        }

        private void OnButtonClicked(object sender, System.EventArgs e)
        {
            ((Button)sender).Text = viewModel.Text;
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