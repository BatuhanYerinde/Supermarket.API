using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services.Communication;
using Supermarket.API.Resources;

namespace Supermarket.API.Domain.Services
{
    public interface IUserService
    {
        Task<UserResponse> SaveAsync(User user);

        Task<TokenResponse> CheckUserExistAndCreateAccessTokenAsync(UserLogin userLogin);

        Task<TokenResponse> CreateAccessTokenByRefreshTokenAsync(string refreshToken); 
    }
}
