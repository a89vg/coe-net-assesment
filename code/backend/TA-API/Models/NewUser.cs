namespace TA_API.Models;

public class NewUser
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string FullName { get; set; }

    public string Password { get; set; }

    public string PasswordConfirmation { get; set; }

    public DateTime DateOfBirth { get; set; }
}

