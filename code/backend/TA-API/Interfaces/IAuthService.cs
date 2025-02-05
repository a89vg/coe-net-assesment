using TA_API.Models;

namespace TA_API.Interfaces;

public interface IAuthService
{
    string GenerateApiToken(string sessionId);
    Task<ResponseModel> Login(UserLoginModel userLogin);
}