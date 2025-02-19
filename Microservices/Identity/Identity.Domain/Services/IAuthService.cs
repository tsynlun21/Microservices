using Infrastructure.Models.Identity;

namespace Identity.Domain.Services;

public interface IAuthService
{
    Task<UserModel> Register(UserRegister userRegister);
    Task<UserModel> Login(UserLogin userLogin);
    
    Task<UserModel> SetUserRole(string userId, string role);
    
    Task<UserModel> GetUserByToken(string token);
}