using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TA_API.Interfaces;
using TA_API.Models;
using TA_API.Models.Data;
using static System.Net.Mime.MediaTypeNames;


namespace TA_API.Controllers;

/// <summary>
/// Users Controller
/// </summary>
[Route("api/users")]
[ApiController]
public class UsersController(IValidator<NewUserModel> newUserValidator, IValidator<UserUpdateModel> userUpdateValidator, IUserService users) : BaseApiController
{
    [HttpGet("")]
    [Authorize(Policy = "User")]
    [ProducesResponseType(typeof(Response<User>), StatusCodes.Status200OK, Application.Json)]
    public async Task<IActionResult> GetUsers()
    {
        var response = await users.GetUsers();

        return Ok(response);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK, Application.Json)]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] NewUserModel user)
    {
        await newUserValidator.ValidateAndThrowAsync(user);

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
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK, Application.Json)]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] UserUpdateModel user)
    {
        await userUpdateValidator.ValidateAndThrowAsync(user);

        var response = await users.UpdateUser(userId, user);

        return Ok(response);
    }

    /// <summary>
    /// Delete an user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK, Application.Json)]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
    {
        var response = await users.DeleteUser(userId);

        return Ok(response);
    }
}