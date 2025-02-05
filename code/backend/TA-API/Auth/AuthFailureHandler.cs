using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using TA_API.Models;
using TA_API.Models.Data;

namespace TA_API.Auth;

public class AuthFailureHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
    private readonly IDistributedCache sessionStorage;

    public AuthFailureHandler(ILogger<AuthFailureHandler> logger, IDistributedCache sessionStorage)
    {
        Logger = logger;
        this.sessionStorage = sessionStorage;
    }

    public ILogger<AuthFailureHandler> Logger { get; }

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        var authorizationFailureReason = authorizeResult.AuthorizationFailure?.FailureReasons.FirstOrDefault();

        if (authorizationFailureReason?.Handler is ApiAuthHandler)
        {
            var response = new ResponseModel
            {
                Message = authorizationFailureReason?.Message
            };

            if (context.User.Identity.IsAuthenticated)
            {
                var sessionId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sid);

                var serializedSession = await sessionStorage.GetStringAsync(sessionId);

                var userSession = JsonSerializer.Deserialize<UserSession>(serializedSession)!;

                Logger.LogWarning("Unauthorized request by {User}", userSession.Username);
            }

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(response, cancellationToken: default);
        }
        else
        {
            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
