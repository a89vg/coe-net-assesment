
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using TA_API.Models.Data;

namespace TA_API.Auth;

public static class MiddlewareExtensions
{
    public static void UseAuthMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<AuthMiddleware>();
    }
}

public class AuthMiddleware : IMiddleware
{
    private readonly IDistributedCache sessionStorage;

    public AuthMiddleware(CurrentUserSessionProvider currentUserSession, IDistributedCache sessionStorage)
    {
        CurrentUserSession = currentUserSession;
        this.sessionStorage = sessionStorage;
    }

    public CurrentUserSessionProvider CurrentUserSession { get; }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var sessionId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sid);

            var serializedSession = await sessionStorage.GetStringAsync(sessionId);

            var userSession = JsonSerializer.Deserialize<UserSession>(serializedSession)!;

            CurrentUserSession.UserSession = userSession;
        }

        await next(context);
    }
}
