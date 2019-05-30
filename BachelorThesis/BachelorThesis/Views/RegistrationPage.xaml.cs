using BachelorThesis.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
        RegistrationViewModel viewModel;
        List<Entry> entries;

        public RegistrationPage ()
		{
			InitializeComponent ();
            viewModel = new RegistrationViewModel();

            MessagingCenter.Subscribe <RegistrationViewModel>(this, "RegisterActivityEnded", (sender) => {
                registerActivity.IsRunning = false;
            });

            this.BindingContext = viewModel;
            entries = new List<Entry>
            {
                lastNameEntry,
                firstNameEntry,
                emailEntry,
                mobilePhoneEntry,
                passwordEntry,
                repeatEntry
            };
		}

        private void OnPhoneNumberChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = Regex.IsMatch(e.NewTextValue,
                 @"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|
                    ^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|
                    ^0)([0-9]{9}$|[0-9\-\s]{10}$)");

            if (e.NewTextValue == "") {
                isValid = true;
                string visualState = isValid ? "Acceptable" : "Invalid";
                VisualStateManager.GoToState(sender as VisualElement, visualState);
                return;
            }

            SetState(isValid, sender as VisualElement);
        }

        private void OnNameChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = Regex.IsMatch(e.NewTextValue,
                 @"^[a-zA-Z]+$");
            SetState(isValid, sender as VisualElement);
        }

        private void OnEmailChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = Regex.IsMatch(e.NewTextValue,
                 @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");

            string visualState = isValid ? "Acceptable" : "Invalid";
            VisualStateManager.GoToState(sender as VisualElement, visualState);
        }

        private void OnRepeatChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = viewModel.IsPasswordsMatches();
            SetState(isValid, sender as VisualElement);
            SetState(isValid, passwordEntry);
        }

        void SetState(bool isValid, VisualElement element)
        {
            string visualState = isValid ? "Valid" : "Invalid";
            VisualStateManager.GoToState(element, visualState);
        }

        private bool IsAllEntriesValid()
        {
            if (entries.Any((x) => x.BackgroundColor == Color.Red))
                return false;
            entries.Remove(mobilePhoneEntry);

            if (entries.Any((x) => x.Text == null))
                return false;

            return true;
        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {
            if (registerActivity.IsRunning) return;
            registerActivity.IsRunning = true;
            if (IsAllEntriesValid())
                viewModel.LoginCommand.Execute(null);
            else registerActivity.IsRunning = false;
        }
    }
}