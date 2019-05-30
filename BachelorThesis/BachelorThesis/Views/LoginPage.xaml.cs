using BachelorThesis.ViewModels;
using System;
using BachelorThesis.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BachelorThesis.Helpers;

namespace BachelorThesis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public LoginViewModel viewModel;
		public LoginPage ()
		{
            viewModel = new LoginViewModel();
			InitializeComponent ();

            TapGestureRecognizer guestTap = new TapGestureRecognizer();
            guestTap.Tapped += (s, e) =>
                {
                    Application.Current.MainPage = new MainPage(null);
                    Settings.IsGuest = true;
                };
            asAGuestLabel.GestureRecognizers.Add(guestTap);

            TapGestureRecognizer registerTap = new TapGestureRecognizer();
            registerTap.Tapped += (s, e) =>
            {
                Navigation.PushModalAsync(new RegistrationPage());
            };
            registrationLabel.GestureRecognizers.Add(registerTap);

            MessagingCenter.Subscribe<LoginViewModel>(this, "LoginActivityEnded", (sender) => {
                loginActivity.IsRunning = false;
            });

            this.BindingContext = viewModel;
		}

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (loginActivity.IsRunning) return;
            loginActivity.IsRunning = true;
            viewModel.LoginCommand.Execute(null);
        }
    }
}