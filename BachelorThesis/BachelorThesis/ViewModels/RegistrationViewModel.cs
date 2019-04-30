using BachelorThesis.Helpers;
using BachelorThesis.Models;
using BachelorThesis.Services;
using BachelorThesis.Views;
using Xamarin.Forms;

namespace BachelorThesis.ViewModels
{
    class RegistrationViewModel : BaseViewModel
    {
        private string errorMessage;
        public Command LoginCommand;

        public User User { get; protected set; }
        public string Repeat { get; set; }
        public RegistrationViewModel()
        {
            User = new User();
            LoginCommand = new Command(Register);
        }

        private async void Register(object obj)
        {
            ErrorMessage = "";
            AuthorizationService service = new AuthorizationService();
            object responce = await service.DoRegisterRequest(User);

            if (responce is ErrorMessage)
            {
                ErrorMessage error = responce as ErrorMessage;

                switch (error.ErrorCode)
                {
                    case 3: ErrorMessage = "Lietotājs ar tādu e-pastu jau eksistē."; break;
                    case 4: ErrorMessage = "Nevarēja izveidot jauno kontu."; break;
                }
            }
            else
            {
                User user = responce as User;
                Settings.IsLoggedIn = true;
                Application.Current.MainPage = new MainPage(user);
            }
            MessagingCenter.Send<RegistrationViewModel>(this, "RegisterActivityEnded");
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        public bool IsPasswordsMatches()
        {
            return User.Password == Repeat;
        }
    }
}
