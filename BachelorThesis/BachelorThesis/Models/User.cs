using System;
using System.Collections.Generic;
using System.Text;

namespace BachelorThesis.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int Avatar { get; set; }
    }
}
