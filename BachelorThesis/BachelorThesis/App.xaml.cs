using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BachelorThesis.Views;
using BachelorThesis.Helpers;
using BachelorThesis.Models;
using Newtonsoft.Json;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BachelorThesis
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            if (Settings.IsLoggedIn)
            {
                User user = JsonConvert.DeserializeObject<User>(Settings.UserSettings);
                MainPage = new MainPage(user);
            }
            else if(Settings.IsGuest){
                MainPage = new MainPage(null);
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
