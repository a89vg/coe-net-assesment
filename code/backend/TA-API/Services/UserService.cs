using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TA_API.Helpers;
using TA_API.Interfaces;
using TA_API.Models;
using TA_API.Models.Data;
using TA_API.Services.Data;

namespace TA_API.Services;

public class UserService(AssessmentDbContext dbContext) : IUserService
{
    public async Task<List<UserModel>> GetUsers()
    {
        var users = await dbContext.Users.ToListAsync();

        return users.Select(u => new UserModel
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FullName = u.FullName,
            DateOfBirth = u.DateOfBirth.ToString("yyyy-MM-dd")
        }).ToList();
    }

    public async Task<ResponseModel> CreateUser(NewUserModel newUser)
    {
        var passwordHasher = new PasswordHasher<NewUserModel>();
        var passwordHash = passwordHasher.HashPassword(newUser, newUser.Password);

        var user = new User
        {
            Username = newUser.Username,
            Email = newUser.Email,
            FullName = newUser.FullName,
            DateOfBirth = DateTime.ParseExact(newUser.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            PasswordHash = passwordHash
        };

        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync();

        return new ResponseModel
        {
            Success = true
        };
    }

    public async Task<ResponseModel> UpdateUser(int userId, UserUpdateModel user)
    {
        var existingUser = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId) ?? throw new ApiException("Record not found", statusCode: 404);

        existingUser.FullName = user.FullName;
        existingUser.DateOfBirth = DateTime.ParseExact(user.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        existingUser.Email = user.Email;

        await dbContext.SaveChangesAsync();

        return new ResponseModel
        {
            Success = true
        };
    }

    public async Task<ResponseModel> DeleteUser(int userId)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId) ?? throw new ApiException("Record not found", statusCode: 404);

        dbContext.Users.Remove(user);

        await dbContext.SaveChangesAsync();

        return new ResponseModel
        {
            Success = true
        };
    }
}