using Microsoft.AspNetCore.Mvc;
using TA_API.Interfaces;
using TA_API.Models;

namespace TA_API.Controllers;

[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService users;

    public UsersController(IUserService users)
    {
        this.users = users;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserLogin user)
    {
        await users.CreateUser(user);
        return Ok();
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, User user)
    {
        await users.UpdateUser(userId, user);
        return Ok();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await users.DeleteUser(userId);
        return Ok();
    }

    [HttpPost("login")]
    public Task<IActionResult> Login(UserLogin userLogin)
    {
        var result = users.Login(userLogin);
        return Ok(result);
    }
}