using BachelorThesis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BachelorThesis.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private User user;
        public User User
        {
            get { return user; }
            protected set { SetProperty(ref user, value); }
        }

        public UserViewModel(User user)
        {
            User = user;
        }
    }
}
