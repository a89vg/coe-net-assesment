using TA_API.Models;

namespace TA_API.Interfaces;

public interface IUserService
{
    Task<List<UserModel>> GetUsers();
    Task<ResponseModel> CreateUser(NewUserModel newUser);
    Task<ResponseModel> UpdateUser(int userId, UserUpdateModel user);
    Task<ResponseModel> DeleteUser(int userId);
}