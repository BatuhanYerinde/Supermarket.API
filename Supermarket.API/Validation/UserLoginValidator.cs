using FluentValidation;
using Supermarket.API.Domain.Models;
using System.Net;

namespace Supermarket.API.Validation
{
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage("Your Email not valid!");
        }
    }
}
