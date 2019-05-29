using BachelorThesis.Helpers;
using BachelorThesis.Models;
using BachelorThesis.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Browse, Title="Galvenā" },
                new HomeMenuItem {Id = MenuItemType.About, Title="Informācija" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

            TapGestureRecognizer tapGesture = new TapGestureRecognizer();

            if (Settings.IsLoggedIn) {
                profileOption.Text = "Iziet";
                tapGesture.Tapped += LogOut;
            } 
            else
            {
                tapGesture.Tapped += LogIn;
            }

            profileOption.GestureRecognizers.Add(tapGesture);
        }

        private void LogIn(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
        private void LogOut(object sender, EventArgs e)
        {
            profileOption.Text = "Autorizēties";
            Settings.IsLoggedIn = false;
            Settings.UserSettings = null;
            this.BindingContext = new UserViewModel(null);
            Label label = sender as Label;
            TapGestureRecognizer tapGesture = label.GestureRecognizers.First() as TapGestureRecognizer;
            tapGesture.Tapped += LogIn;
        }
    }
}