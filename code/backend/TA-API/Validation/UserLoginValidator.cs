using FluentValidation;
using TA_API.Models;

namespace TA_API.Validation;

public class UserLoginValidator : BaseValidator<UserLoginModel>
{
    public UserLoginValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(user => user)
            .NotNull();

        RuleFor(user => user.Username)
            .NotEmpty()
            .WithMessage("Please enter a Username");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Please enter your Password");
    }
}
