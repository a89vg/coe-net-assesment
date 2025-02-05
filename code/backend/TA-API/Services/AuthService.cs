using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using TA_API.Interfaces;
using TA_API.Models.Data;
using TA_API.Models;
using TA_API.Services.Data;
using Microsoft.Extensions.Caching.Distributed;
using TA_API.Auth;

namespace TA_API.Services;

public class AuthService(AuthConfig authConfig, AssessmentDbContext dbContext, IDistributedCache sessionStorage) : IAuthService
{
    public string GenerateApiToken(string sessionId)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.SigningKey));

        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: authConfig.Issuer,
                audience: authConfig.Audience,
                claims: [new(JwtRegisteredClaimNames.Sid, sessionId)],
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ResponseModel> Login(UserLoginModel userLogin)
    {
        var user = await dbContext.Users.Where(u => u.Username.Equals(userLogin.Username)).SingleOrDefaultAsync();

        if (user is null)
        {
            return new ResponseModel { Message = "Wrong username and/or password" };
        }

        var passwordHasher = new PasswordHasher<User>();

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLogin.Password);

        if (result != PasswordVerificationResult.Success)
        {
            return new ResponseModel { Message = "Wrong username and/or password" };
        }

        var userSession = new UserSession
        {
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email
        };

        var roles = await dbContext.UserRoles.Where(u => u.Username == userLogin.Username).Select(r => r.Role).ToListAsync();

        if (roles.Count == 0)
        {
            return new ResponseModel { Message = "User does not have a role assigned" };
        }

        userSession.Roles = roles;

        var sessionId = Guid.NewGuid().ToString();

        await sessionStorage.SetStringAsync(sessionId, JsonSerializer.Serialize(userSession));

        return new ResponseModel { Success = true, SID = sessionId };
    }
}
