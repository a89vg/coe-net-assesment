using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using TA_API.Models;
using TA_API.Models.Data;

namespace TA_API.Auth;

public static class MiddlewareExtensions
{
    public static void UseAuthMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<SessionProviderMiddleware>();
    }
}

public class SessionProviderMiddleware(CurrentUserSessionProvider currentUserSession, IDistributedCache sessionStorage) : IMiddleware
{
    public CurrentUserSessionProvider CurrentUserSession { get; } = currentUserSession;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var sessionId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sid);

            var serializedSession = await sessionStorage.GetStringAsync(sessionId);

            if (serializedSession is not null)
            {
                var userSession = JsonSerializer.Deserialize<UserSession>(serializedSession)!;

                CurrentUserSession.UserSession = userSession;
            }
            else
            {
                var response = new ResponseModel
                {
                    Message = "Session has expired. Please log in again"
                };

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(response, cancellationToken: default);
            }
        }

        await next(context);
    }
}
