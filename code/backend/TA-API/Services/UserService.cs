using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TA_API.Helpers;
using TA_API.Interfaces;
using TA_API.Models;
using TA_API.Models.Data;
using TA_API.Services.Data;

namespace TA_API.Services;

public class UserService : IUserService
{
    private readonly AssessmentDbContext dbContext;
    private readonly IDistributedCache sessionStorage;

    public UserService(AssessmentDbContext dbContext, IDistributedCache cache)
    {
        this.dbContext = dbContext;
        sessionStorage = cache;
    }

    public async Task<List<User>> GetUsers()
    {
        try
        {
            var users = dbContext.Users.ToList();

            return users;
        }
        catch (Exception ex)
        {
            ex.LogError("Error getting users");

            throw;
        }
    }

    public async Task<Response> CreateUser(NewUser newUser)
    {
        try
        {
            if (!string.IsNullOrEmpty(newUser.PasswordConfirmation) && newUser.Password != newUser.PasswordConfirmation)
            {
                return new Response { Message = "Password and confirmation do nto match" };
            }

            var passwordHasher = new PasswordHasher<NewUser>();
            var passwordHash = passwordHasher.HashPassword(newUser, newUser.Password);

            var user = new User
            {
                Username = newUser.Username,
                PasswordHash = passwordHash
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return new Response
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            ex.LogError("Error creating user");

            throw;
        }
    }

    public async Task<Response> UpdateUser(int userId, User user)
    {
        try
        {
            var existingUser = dbContext.Users.Where(u => u.Id == userId).SingleOrDefault();

            //TODO: Update user

            await dbContext.SaveChangesAsync();

            return new Response
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            ex.LogError("Error updating user");

            throw;
        }
    }

    public async Task<Response> DeleteUser(int userId)
    {
        try
        {
            var user = dbContext.Users.Where(u => u.Id.Equals(userId)).SingleOrDefault();

            dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();

            return new Response
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            ex.LogError("Error deleting user");

            throw;
        }
    }

    public async Task<Response> Login(UserLogin userLogin)
    {
        try
        {
            var user = dbContext.Users.Where(u => u.Username.Equals(userLogin.Username)).SingleOrDefault();

            if (user is null)
            {
                return new Response { Message = "Wrong username and/or password" };
            }

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLogin.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return new Response { Message = "Wrong username and/or password" };
            }

            var userSession = new UserSession
            {
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email
            };

            var roles = dbContext.UserRoles.Where(u => u.Username == userLogin.Username).Select(r => r.Role).ToList();

            if (roles.Count == 0)
            {
                return new Response { Message = "User does not have a role assigned" };
            }

            userSession.Roles = roles;

            var sessionId = Guid.NewGuid().ToString();

            await sessionStorage.SetStringAsync(sessionId, JsonSerializer.Serialize(userSession));

            return new Response { Success = true, SID = sessionId };
        }
        catch (Exception ex)
        {
            ex.LogError("Error on user login");

            throw;
        }
    }
}