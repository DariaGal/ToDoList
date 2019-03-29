using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Users
{
    public interface IUserService
    {
        Task<User> CreateAsync(UserInfo creationInfo, CancellationToken cancellationToken);

        Task<User> GetAsync(string login, CancellationToken cancellationToken);

       Task<User> GetAsync(Guid userId, CancellationToken cancellationToken);
    }
}
