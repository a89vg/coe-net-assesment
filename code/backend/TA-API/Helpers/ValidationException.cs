using TA_API.Models;

namespace TA_API.Helpers;


public class ValidationException : ApiException
{
    public ValidationException(string errorMessage) : base(errorMessage, LogLevel.Warning, 400)
    {
        ErrorResponse = new ResponseModel
        {
            Message = "API Validation Error",
            ErrorDetails = errorMessage
        };
    }
}
