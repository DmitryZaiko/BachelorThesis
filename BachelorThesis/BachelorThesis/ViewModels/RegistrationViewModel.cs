using BachelorThesis.Models;

namespace BachelorThesis.ViewModels
{
    class RegistrationViewModel : BaseViewModel
    {
        public User User { get; protected set; }
        public string Repeat { get; set; }
        public RegistrationViewModel()
        {
            User = new User();
            
        }

        public bool IsPasswordsMatches()
        {
            return User.Password == Repeat;
        }
    }
}
