using FluentValidation;
using FluentValidation.Results;
using ValidationException = TA_API.Helpers.ValidationException;

namespace TA_API.Validation;

public class BaseValidator<T> : AbstractValidator<T>
{
    public BaseValidator()
    {

    }

    protected override void RaiseValidationException(ValidationContext<T> context, ValidationResult result)
    {
        throw new ValidationException(result.ToString(" | "));
    }
}
