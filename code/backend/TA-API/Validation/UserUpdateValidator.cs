using FluentValidation;
using System.Globalization;
using TA_API.Models;

namespace TA_API.Validation;

public class UserUpdateValidator : BaseValidator<UserUpdateModel>
{
    const string MissingValueMessage = "Please enter {PropertyName}";

    public UserUpdateValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(MissingValueMessage)
            .EmailAddress().WithMessage("{PropertyValue} is not a valid email");

        RuleFor(user => user.DateOfBirth)
            .NotEmpty().WithName("Date of Birth").WithMessage(MissingValueMessage)
            .Custom((dob, ctx) =>
            {
                if (!DateTime.TryParseExact(dob, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOfBirth) || dateOfBirth.Date > DateTime.Now.Date)
                {
                    throw new ValidationException("The supplied Date of Birth is not valid)");
                }
            });

        RuleFor(user => user.FullName)
            .NotEmpty().WithName("your full name");
    }
}
