using System.Text.Json.Serialization;

namespace TA_API.Models;

public class ResponseModel
{
    public ResponseModel() => Success = false;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ApiToken { get; set; } = null;
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string SID { get; set; }
    public bool Success { get; set; } = false;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; } = null;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Code { get; set; } = null;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? Count { get; set; } = null;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorId { get; set; } = null;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorDetails { get; set; } = null;

}

public class Response<T> : ResponseModel
{
    public Response() { }

    public Response(T result) => Result = result;

    public Response(List<T> result)
    {
        List = result;
        Count = List.Count;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Result { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<T>? List { get; set; } = null;
}
