using Supermarket.API.Domain.Models;

namespace Supermarket.API.Domain.Services.Communication
{
    public class UserResponse : BaseResponse<User>
    {
        public UserResponse(User user) : base(user) { }

        public UserResponse(string message) : base(message) { }
    }
}
