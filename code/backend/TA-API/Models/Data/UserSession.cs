namespace TA_API.Models.Data;

public class UserSession
{
    public string Username { get; set; }

    public string Email { get; set; }

    public List<string> Roles { get; set; }

    public string FullName { get; set; }
}
