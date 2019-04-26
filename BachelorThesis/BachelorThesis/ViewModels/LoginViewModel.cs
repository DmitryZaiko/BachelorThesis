using BachelorThesis.Helpers;
using BachelorThesis.Models;
using BachelorThesis.Services;
using BachelorThesis.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BachelorThesis.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public User User { get; protected set; }
        private string errorMessage;
        private string emailInput;
        private string passwordInput;
        public Command LoginCommand;

        public LoginViewModel()
        {
            LoginCommand = new Command(Login);   
        }

        private async void Login(object obj)
        {
            ErrorMessage = "";
            AuthorizationService service = new AuthorizationService();
            object responce = await service.DoLoginRequest(EmailInput, PasswordInput);

            if (responce is LoginError)
            {
                LoginError error = responce as LoginError;

                switch(error.ErrorCode)
                {
                    case 0: ErrorMessage = "Nevarēja atrast jūsu kontu."; break; 
                    case 1: ErrorMessage = "Parole nav pareiza."; break;
                }
            }
            else
            {
                User user = responce as User;
                Settings.IsLoggedIn = true;
                Application.Current.MainPage = new MainPage(user);
            }

        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }
        public string EmailInput
        {
            get { return emailInput; }
            set { SetProperty(ref emailInput, value); }
        }

        public string PasswordInput
        {
            get { return passwordInput; }
            set { SetProperty(ref passwordInput, value); }
        }
    }
}
