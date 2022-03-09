using FluentValidation;
using KonOgren.Infrastructure.ViewModel;

namespace KonOgren.Infrastructure.FluentValidations
{
   public class UserValidator : AbstractValidator<LoginViewModel>
    {
        public UserValidator()
        {
            RuleFor(d => d.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(d => d.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
