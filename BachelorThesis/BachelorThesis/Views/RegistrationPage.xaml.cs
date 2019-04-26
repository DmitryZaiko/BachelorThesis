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
		public RegistrationPage ()
		{
			InitializeComponent ();
            viewModel = new RegistrationViewModel();
            this.BindingContext = viewModel;
		}

        private void OnPhoneNumberChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = Regex.IsMatch(e.NewTextValue,
                 @"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|
                    ^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|
                    ^0)([0-9]{9}$|[0-9\-\s]{10}$)");
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
            SetState(isValid, sender as VisualElement);
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

    }
}