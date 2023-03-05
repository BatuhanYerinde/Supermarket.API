using Supermarket.API.Domain.Models;

namespace Supermarket.API.Domain.Services.Communication
{
    public class TokenResponse : BaseResponse<Token>
    {
        public TokenResponse(Token token) : base(token) { }

        public TokenResponse(string message) : base(message) { }
    }
}
