namespace TA_API.Models;

public class User
{
    public int Id { get; set;}

    public string Username { get; set;}

    public string PasswordHash { get; set; }
}

public class UserLogin 
{
    public string Username { get; set; }
    public string Password { get; set; }
}

