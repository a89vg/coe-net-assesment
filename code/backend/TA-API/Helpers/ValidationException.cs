namespace TA_API.Helpers;


public class ValidationException(string errorMessage) : Exception
{
    public ValidationError ValidationError { get; } = new ValidationError(errorMessage);
}
