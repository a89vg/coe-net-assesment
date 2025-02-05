using TA_API.Models;

namespace TA_API.Helpers;

public class ApiException : Exception
{
    public ApiException(string errorMessage, LogLevel severity = LogLevel.Error, int statusCode = 500) : base(errorMessage)
    {
        ErrorResponse = new ResponseModel
        {
            Message = "Error in API",
            ErrorDetails = errorMessage
        };
        Severity = severity;
        StatusCode = statusCode;
    }

    public ResponseModel ErrorResponse { get; set; }
    public LogLevel Severity { get; }
    public int StatusCode { get; }
}
