using TA_API.Models;
using TA_API.Models.Data;

namespace TA_API.Interfaces;

public interface IUserService
{
    Task<List<User>> GetUsers();
    Task<Response> CreateUser(NewUser userLogin);
    Task<Response> UpdateUser(int userId, User user);
    Task<Response> DeleteUser(int userId);
    Task<Response> Login(UserLogin userLogin);
}