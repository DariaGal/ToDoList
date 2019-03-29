using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Users
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid userId)
            : base($"A user by id \"{userId}\" is not found.")
        {
        }
        
        public UserNotFoundException(string login)
            : base($"A user by login \"{login}\" is not found.")
        {
        }
    }
}
