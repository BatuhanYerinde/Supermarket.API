using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Repositories;
using Supermarket.API.Domain.Services;
using Supermarket.API.Domain.Services.Communication;
using Supermarket.API.Handler;
using Supermarket.API.Resources;

namespace Supermarket.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenService _tokenService; 
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, TokenService tokenService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        public async Task<UserResponse> SaveAsync(User user)
        {
            try
            {
                await _userRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(user);
            }
            catch (Exception ex)
            {
                return new UserResponse($"An error occurred when saving the user: {ex.Message}");
            }
        }

        public async Task<TokenResponse> CheckUserExistAndCreateAccessTokenAsync(UserLogin userLogin)
        {
            try
            {
                var user = await _userRepository.CheckUserExistAsync(userLogin);
                if (user != null) {
                    return await CreateAccessTokenAsync(user);
                }
                return new TokenResponse($"User not found!");
            }
            catch (Exception ex)
            {
                return new TokenResponse($"An error occurred when check user exist: {ex.Message}");
            }
        }

        public async Task<TokenResponse> CreateAccessTokenAsync(User user)
        {
            try
            {
                var token = _tokenService.CreateAccessToken(user);
                _userRepository.Update(user);
                await _unitOfWork.CompleteAsync();

                return new TokenResponse(token);
            }
            catch (Exception ex) 
            {
                return new TokenResponse($"Failed to create token: {ex.Message}" );
            }
        }

        public async Task<TokenResponse> CreateAccessTokenByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _userRepository.CheckUserExistAsync(refreshToken);
                if (user != null && user.RefreshTokenEndDate > DateTime.Now)
                {
                    return await CreateAccessTokenAsync(user);
                }
                return new TokenResponse($"User not found or refresh token expired!");
            }
            catch (Exception ex)
            {
                return new TokenResponse($"An error occurred when check user exist: {ex.Message}");
            }
        }
    }
}
