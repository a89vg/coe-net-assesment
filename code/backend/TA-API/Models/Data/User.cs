using System.Text.Json.Serialization;

namespace TA_API.Models.Data;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string FullName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string PasswordHash { get; set; }

    public DateTime DateOfBirth { get; set; }
}