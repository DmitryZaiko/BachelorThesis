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
using System.Collections;

namespace BachelorThesis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        bool HaveQuizes { get; set; } = false;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel(new URLHttpParams("Course")){
                                                        PageType = ItemType.Course,
                                                        Navigation = this.Navigation};
        }

        public ItemsPage(URLHttpParams httpParams, ItemType pageType)
        {
            InitializeComponent();

            viewModel = new ItemsViewModel(httpParams)
                        {
                            PageType = pageType,
                            Navigation = this.Navigation
                        };

            BindingContext = viewModel;

            if (pageType == ItemType.Content)
            {
                stackLayout.Children.RemoveAt(0);
                quizButton.IsVisible = true;

                gridView.ColumnDefinitions.Add(new ColumnDefinition()
                { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumnSpan(stackLayout, 3);
                gridView.Children.Add(quizButton, 2, 1);

                WebView webView = new WebView();
                webView.HorizontalOptions = LayoutOptions.FillAndExpand;
                webView.VerticalOptions = LayoutOptions.FillAndExpand;

                var source = new HtmlWebViewSource();
                source.SetBinding(HtmlWebViewSource.HtmlProperty, "Text");
                webView.Source = source;

                stackLayout.Children.Add(webView);

                MessagingCenter.Subscribe<ItemsViewModel, object>(this, "ContentLoaded", (seder, obj) =>
                {
                    Content info = ((IEnumerable)obj).Cast<Content>().First();
                    if (info.Count > 0)
                        quizButton.IsEnabled = true;
                });
            }   
        }

        async void OnQuizButtonClicked(object sender, System.EventArgs e)
        {
            string id = viewModel.Items.First().Id.ToString();
            URLHttpParams httpParams = new URLHttpParams(ItemType.Quiz, id);
            await Navigation.PushModalAsync(new QuizPage(httpParams));
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            ItemType nextPageType;
            URLHttpParams httpParams;
            string title;

            switch (viewModel.PageType)
            {
                case ItemType.Course:
                    nextPageType = ItemType.Lesson;
                    title = "Nodarbības";
                    break;
                case ItemType.Lesson:
                    nextPageType = ItemType.Content;
                    title = item.Name;
                    break;
                default:
                    nextPageType = ItemType.Course;
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