using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using TA_API.Models.Data;

namespace TA_API.Auth;

public class ApiAuthHandler(IDistributedCache sessionStorage) : AuthorizationHandler<RoleRequirement>()
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Fail(new AuthorizationFailureReason(this, "User is not authenticated"));

            return;
        }

        var sessionId = context.User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;

        var serializedSession = await sessionStorage.GetStringAsync(sessionId);

        var currentUserSession = JsonSerializer.Deserialize<UserSession>(serializedSession);

        if (currentUserSession.Roles.Contains(requirement.Role))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, "Action is forbidden"));
        }
    }
}
