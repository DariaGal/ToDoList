using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Users
{
    public interface IUserRepository
    {
        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="creationInfo">Данные для создания нового пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданный пользователь</returns>
        Task<User> CreateAsync(UserInfo creationInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Пользователь</returns>
        Task<User> GetAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Пользователь</returns>
        Task<User> GetAsync(string login, CancellationToken cancellationToken);
    }
}
