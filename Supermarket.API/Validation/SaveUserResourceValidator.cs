using FluentValidation;
using Supermarket.API.Resources;

namespace Supermarket.API.Validation
{
    public class SaveUserResourceValidator : AbstractValidator<SaveUserResource>
    {
        public SaveUserResourceValidator()
        {
            RuleFor(user => user.Name).NotEmpty().Length(2, 250);
            RuleFor(user => user.Surname).NotEmpty().Length(2, 250);
            RuleFor(user => user.Email).EmailAddress().WithMessage("Your Email not valid!");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                    .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
            // code repeated in another validator
        }
    }
}
