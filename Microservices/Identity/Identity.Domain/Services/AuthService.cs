using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Domain.EntititesContext;
using Infrastructure.Exceptions;
using Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Domain.Services;

public class AuthService(IOptions<AuthOptions> authOptions, UserManager<UserEntity> userManager) : IAuthService
{
    private readonly AuthOptions _authOptions = authOptions.Value;
    
    public async Task<UserModel> Register(UserRegister userRegister)
    {
        if (await userManager.FindByNameAsync(userRegister.UserName) != null)
        {
            throw new BadRequestException($"Username [{userRegister.UserName}] is already taken.");
        }

        var createUserResult = await userManager.CreateAsync(new UserEntity()
        {
            Email = userRegister.Email,
            PhoneNumber = userRegister.Phone,
            UserName = userRegister.UserName
        }, userRegister.Password);

        if (createUserResult.Succeeded)
        {
            var user = await userManager.FindByEmailAsync(userRegister.Email);
            
            if (user == null)
                throw new NotFoundException($"User with email {userRegister.Email} not registered.");

            var roleAssigning = await userManager.AddToRoleAsync(user, RoleConstants.User);
            
            if (roleAssigning.Succeeded == false)
                throw new BadRequestException($"Error registering user role [{roleAssigning}].");

            var userResponse = new UserModel()
            {
                Id = user.Id,
                Email = user.Email,
                Roles = [RoleConstants.User],
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };
            
            return GenerateToken(userResponse);
        }
        
        throw new BadRequestException($"Error registering user : [{createUserResult.ToString()}].");
    }

    public async Task<UserModel> Login(UserLogin userLogin)
    {
        var user = await userManager.FindByNameAsync(userLogin.UserName);

        if (user == null)
            throw new BadRequestException($"User [{userLogin.UserName}] not found.");
        
        var checkPasswordResult = await userManager.CheckPasswordAsync(user, userLogin.Password);

        if (checkPasswordResult == false)
        {
            throw new BadRequestException("Invalid password.");
        }
        
        var userRoles = await userManager.GetRolesAsync(user);
        var userResponse = new UserModel()
        {
            Id          = user.Id,
            Email       = user.Email,
            Roles       = userRoles,
            UserName    = user.UserName,
            PhoneNumber = user.PhoneNumber
        };
        return GenerateToken(userResponse);
    }

    public async Task<UserModel> SetUserRole(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
            throw new BadRequestException($"User [{userId}] not found.");
        
        if (role != RoleConstants.User || role != RoleConstants.Admin || role != RoleConstants.Merchant)
            throw new BadRequestException("Invalid role.");
        
        var res = await userManager.AddToRoleAsync(user, role.ToString());

        return new UserModel()
        {
            Id          = user.Id,
            Email       = user.Email,
            Roles       = [role],
            UserName    = user.UserName,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<UserModel> GetUserByToken(string token)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authOptions.TokenPrivateKey);
        var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            IssuerSigningKey         = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ValidateIssuer           = false,
            ValidateAudience         = false,
            ValidateLifetime         = true
        });
        
        if (!result.IsValid)
            throw new BadRequestException("Invalid token.");
        
        var id = result.Claims.FirstOrDefault(c => c.Key == "id").Value.ToString();
        
        var user = await GetUserById(id!);

        return user;
    }

    public async Task<UserModel> GetUserById(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            throw new BadRequestException($"User [{userId}] not found.");

        var roles = await userManager.GetRolesAsync(user);
        
        return new UserModel()
        {
            Id          = user.Id,
            Email       = user.Email,
            UserName    = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Roles = roles
        };
    }

    private UserModel GenerateToken(UserModel userModel)
    {
        var handler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_authOptions.TokenPrivateKey);
        var creds = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject            = GenerateClaims(userModel),
            Expires            = DateTime.UtcNow.AddMinutes(_authOptions.ExpireIntervalInMinutes),
            SigningCredentials = creds
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        userModel.Token = handler.WriteToken(token);
        
        return userModel;
    }

    private ClaimsIdentity GenerateClaims(UserModel userModel)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim("id", userModel.Id));
        claims.AddClaim(new Claim(ClaimTypes.Name, userModel.UserName));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString()));

        foreach (var role in userModel.Roles!)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        
        return claims;
    }
}