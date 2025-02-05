namespace TA_API.Auth;

public class AuthConfig
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string SigningKey { get; set; }
}
