using Microsoft.AspNetCore.Identity;
using TA_API.Interfaces;
using TA_API.Models;
using TA_API.Services.Data;

namespace TA_API.Services;

public class UserService : IUserService
{
    private readonly AssessmentDbContext dbContext;

    public UserService(AssessmentDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateUser(UserLogin userLogin)
    {
        var passwordHasher = new PasswordHasher<UserLogin>();
        var passwordHash = passwordHasher.HashPassword(userLogin, userLogin.Password);

        var user = new User
        {
            Username = userLogin.Username,
            PasswordHash = passwordHash
        };

        await dbContext.Users.AddAsync(user);        
        await dbContext.SaveChangesAsync();

        return;
    }

    public async Task UpdateUser(int userId, User user)
    {
        var existingUser = dbContext.Users.Where(u => u.Id == userId).SingleOrDefault();

        //TODO: Update user

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(int userId)
    {
        var user = dbContext.Users.Where(u => u.Id.Equals(userId)).SingleOrDefault();

        dbContext.Users.Remove(user);

        await dbContext.SaveChangesAsync();
    }

    public bool Login(UserLogin userLogin)
    {
        var user = dbContext.Users.Where(u => u.Username.Equals(userLogin.Username)).SingleOrDefault();

        var passwordHasher = new PasswordHasher<UserLogin>();

        var result = passwordHasher.VerifyHashedPassword(userLogin, user.PasswordHash, userLogin.Password);

        if(result == PasswordVerificationResult.Success)
        {
            return true;
        }

        return false;
    }
}