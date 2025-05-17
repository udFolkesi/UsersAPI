using Core.Entities;
using FluentValidation;

namespace BLL.Validation
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login is required")
                .Matches("^[a-zA-Z0-9]+$").WithMessage("Login can contain only Latin letters and digits");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Matches("^[a-zA-Z0-9]+$").WithMessage("Password can contain only Latin letters and digits");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Matches("^[a-zA-Zа-яА-ЯёЁ]+$").WithMessage("Name can contain only Latin and Cyrillic letters");
        }
    }
}
