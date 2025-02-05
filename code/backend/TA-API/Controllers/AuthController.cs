using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TA_API.Filters;
using TA_API.Interfaces;
using TA_API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TA_API.Controllers;

/// <summary>
/// Security Controller
/// </summary>
[Route("api/auth")]
[ApiController]
[ServiceFilter<ErrorHandlingFilter>]
public class AuthController(IValidator<UserLoginModel> loginValidator, IAuthService authService) : BaseApiController
{

    /// <summary>
    /// User Login
    /// </summary>
    /// <param name="userLogin"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK, Application.Json)]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginModel userLogin)
    {
        await loginValidator.ValidateAndThrowAsync(userLogin);

        var response = await authService.Login(userLogin);

        if (response.Success)
        {
            var token = authService.GenerateApiToken(response.SID);

            response.ApiToken = token;
        }

        return Ok(response);
    }
}
