using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Users
{
    public class UserInfo
    {

        /// <summary>
        /// Инийиализирует новый экземпляр описания для создания пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="passwordHash">Хэш пароля</param>
        public UserInfo(string login, string passwordHash)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (passwordHash == null)
            {
                throw new ArgumentNullException(nameof(passwordHash));
            }

            this.Login = login;
            this.PasswodHash = passwordHash;
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string PasswodHash { get; }
    }
}
