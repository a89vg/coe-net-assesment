using Microsoft.AspNetCore.Authorization;

namespace TA_API.Auth;

public class RoleRequirement : IAuthorizationRequirement
{
    public RoleRequirement(string role)
    {
        Role = role;
    }

    public string Role { get; }
}
