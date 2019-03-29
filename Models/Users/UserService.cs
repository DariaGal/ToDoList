using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Models.Users
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("ToDoListDB"));
            var database = client.GetDatabase("ToDoListDB");
            _users = database.GetCollection<User>("Users");
        }

        public Task<User> CreateAsync(UserInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var filter = new BsonDocument("Login", creationInfo.Login);
            if (_users.CountDocuments(filter) > 0)
            {
                throw new UserDuplicationException(creationInfo.Login);
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

            _users.InsertOne(user);
            return Task.FromResult(user);
        }

        public Task<User> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = _users.Find(u => u.Id == userId).FirstOrDefault();
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return Task.FromResult(user);
        }

        public Task<User> GetAsync(string login, CancellationToken cancellationToken)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var user = _users.Find(u => u.Login == login).FirstOrDefault();
            if (user == null)
            {
                throw new UserNotFoundException(login);
            }

            return Task.FromResult(user);
        }
    }
}
