using System;
using System.Collections.Generic;
using System.Text;

namespace _3iRegistry.Core
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public string Token { get; set; }
    }
}
