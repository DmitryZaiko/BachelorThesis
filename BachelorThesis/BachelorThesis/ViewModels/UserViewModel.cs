using BachelorThesis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BachelorThesis.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private User user;
        private string fullUserName;

        public User User
        {
            get { return user; }
            protected set { SetProperty(ref user, value); }
        }

        public string FullUserName
        {
            get { return fullUserName; }
            protected set { SetProperty(ref fullUserName, value); }
        }

        public UserViewModel(User user)
        {
            User = user;
            FullUserName = user != null ? User.FirstName + " " + User.LastName : "Guest";
        }
    }
}
