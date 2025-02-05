using FluentValidation;
using System.Globalization;
using TA_API.Models;

namespace TA_API.Validation;

public class NewUserValidator : BaseValidator<NewUserModel>
{
    const string MissingValueMessage = "Please enter {PropertyName}";
    const string MinLengthMessage = "{PropertyName} must be at least {MinLength} characters long. (Current: {TotalLength})";
    const string MaxLengthMessage = "{PropertyName} must be at most {MaxLength} characters long. (Current: {TotalLength})";

    public NewUserValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage(MissingValueMessage)
            .MinimumLength(5).WithMessage(MinLengthMessage)
            .MaximumLength(16).WithMessage(MaxLengthMessage);

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage(MissingValueMessage)
            .MinimumLength(6).WithMessage(MinLengthMessage)
            .MaximumLength(16).WithMessage(MaxLengthMessage);

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(MissingValueMessage)
            .EmailAddress().WithMessage("{PropertyValue} is not a valid email");

        RuleFor(user => user.PasswordConfirmation)
            .NotEmpty().WithMessage("Please confirm your password")
            .Equal(user => user.Password).WithMessage("Password and confirmation do not match");

        RuleFor(user => user.DateOfBirth)
            .NotEmpty().WithName("Date of Birth").WithMessage(MissingValueMessage)
            .Custom((dob, ctx) =>
            {
                if (!DateTime.TryParseExact(dob, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOfBirth) || dateOfBirth.Date > DateTime.Now.Date)
                {
                    ctx.AddFailure("The supplied Date of Birth is not valid");
                }
            });

        RuleFor(user => user.FullName)
            .NotEmpty().WithName("your full name");
    }    
}
