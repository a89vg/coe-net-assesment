using TA_API.Models;

namespace TA_API.Interfaces;

public interface IUserService
{
    Task CreateUser(UserLogin userLogin);
    Task UpdateUser(int userId, User user);
    Task DeleteUser(int userId);
    bool Login(UserLogin userLogin);
}