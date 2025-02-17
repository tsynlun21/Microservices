using Infrastructure.Models.Identity;

namespace Identity.Domain.Services;

public interface IAuthService
{
    Task<UserResponse> Register(UserRegister userRegister);
    Task<UserResponse> Login(UserLogin userLogin);
    
    Task SetUserRole(int userId, string role);
}