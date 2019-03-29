using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Users
{
    public class UserDuplicationException : Exception
    {
        public UserDuplicationException(string login)
          : base($"A user with lofgin \"{login}\" is already exist.")
        {
        }
    }
}
