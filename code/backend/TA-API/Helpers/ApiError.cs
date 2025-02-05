using System.Text.Json.Serialization;

namespace TA_API.Helpers;

public class ApiError
{
    public ApiError()
    {

    }

    public ApiError(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    [JsonPropertyOrder(1)]
    public string ErrorMessage { get; set; }
}
