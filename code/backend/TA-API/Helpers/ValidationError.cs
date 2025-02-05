namespace TA_API.Helpers;

public class ValidationError : ApiError
{
    public ValidationError(string errorMessage) : base(errorMessage)
    {
            
    }
}
