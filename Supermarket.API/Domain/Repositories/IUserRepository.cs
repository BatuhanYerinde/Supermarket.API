using Supermarket.API.Domain.Models;
using Supermarket.API.Resources;

namespace Supermarket.API.Domain.Repositories
{
    public interface IUserRepository 
    {
        Task AddAsync(User user);

        Task<User> CheckUserExistAsync(UserLogin userLogin);

        Task<User> CheckUserExistAsync(string refreshToken);

        void Update(User user);
    }
}
