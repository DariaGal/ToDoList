using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Users
{
    public class UserRepository : IUserRepository
    {

        private readonly Dictionary<Guid, User> primaryKeyIndex;
        private readonly Dictionary<string, User> loginIndex;

        /// <summary>
        /// Инициализирует новый эксземлпляр хранилища пользователей в памяти
        /// </summary>
        public UserRepository()
        {
            this.primaryKeyIndex = new Dictionary<Guid, User>();
            this.loginIndex = new Dictionary<string, User>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="creationInfo">Данные для создания нового пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданный пользователь</returns>
        public Task<User> CreateAsync(UserInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (this.loginIndex.ContainsKey(creationInfo.Login))
            {
              //  throw new UserDuplicationException(creationInfo.Login);
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var user = new User
            {
                Id = id,
                Login = creationInfo.Login,
                PasswordHash = creationInfo.PasswodHash,
                RegisteredAt = now
            };

            this.primaryKeyIndex.Add(id, user);
            this.loginIndex.Add(user.Login, user);

            return Task.FromResult(user);
        }

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Пользователь</returns>
        public Task<User> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!this.primaryKeyIndex.TryGetValue(userId, out var user))
            {
                //throw new UserNotFoundException(userId);
            }

            return Task.FromResult(user);
        }

        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Пользователь</returns>
        public Task<User> GetAsync(string login, CancellationToken cancellationToken)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (!this.loginIndex.TryGetValue(login, out var user))
            {
               // throw new UserNotFoundException(login);
            }

            return Task.FromResult(user);
        }
    }
}
