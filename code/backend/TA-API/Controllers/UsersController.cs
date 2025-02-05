using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TA_API.Auth;
using TA_API.Filters;
using TA_API.Interfaces;
using TA_API.Models;
using TA_API.Models.Data;
using static System.Net.Mime.MediaTypeNames;

namespace TA_API.Controllers;

/// <summary>
/// Users Controller
/// </summary>
[Route("users")]
[ApiController]
[ServiceFilter<ErrorHandlingFilter>]
public class UsersController : ControllerBase
{
    private readonly IUserService users;
    private readonly IConfiguration config;
    private readonly UserSession currentUserSession;

    public ILogger<UsersController> Logger { get; }

    public UsersController(IUserService users, CurrentUserSessionProvider currentUserSession, IDistributedCache cache, IConfiguration config, ILogger<UsersController> logger)
    {
        this.users = users;
        this.config = config;
        Logger = logger;
        this.currentUserSession = currentUserSession.UserSession;
    }

    [HttpGet("")]
    [Authorize(Policy = "User")]
    [ProducesResponseType(typeof(Response<User>), StatusCodes.Status200OK, Application.Json)]
    public async Task<IActionResult> GetUsers()
    {
        Logger.LogInformation("GetUsers called by {CurrentUser}", currentUserSession.Username);

        var response = await users.GetUsers();

        return Ok(response);
    }


    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK, Application.Json)]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateUser(NewUser user)
    {
        var response = await users.CreateUser(user);

        return Ok(response);
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPut("{userId}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK, Application.Json)]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateUser(int userId, User user)
    {
        var response = await users.UpdateUser(userId, user);

        return Ok(response);
    }

    /// <summary>
    /// Delete an user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK, Application.Json)]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var response = await users.DeleteUser(userId);

        return Ok(response);
    }

    /// <summary>
    /// User Login
    /// </summary>
    /// <param name="userLogin"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK, Application.Json)]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLogin userLogin)
    {
        var response = await users.Login(userLogin);

        if (response.Success)
        {
            var token = GenerateApiToken(response.SID);

            response.ApiToken = token;
        }

        return Ok(response);
    }

    private string GenerateApiToken(string sessionId)
    {
        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AuthConfig")["SigningKey"])), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: config.GetSection("AuthConfig")["Issuer"],
                audience: config.GetSection("AuthConfig")["Audience"],
                claims: [new(JwtRegisteredClaimNames.Sid, sessionId)],
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}