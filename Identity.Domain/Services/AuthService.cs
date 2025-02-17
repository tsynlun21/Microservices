using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.DataAccess.DataContext;
using Identity.DataAccess.EntititesContext;
using Infrastructure.Exceptions;
using Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Domain.Services;

public class AuthService(IOptions<AuthOptions> authOptions, UserManager<UserEntity> userManager) : IAuthService
{
    private readonly AuthOptions _authOptions = authOptions.Value;
    
    public async Task<UserResponse> Register(UserRegister userRegister)
    {
        if (await userManager.FindByEmailAsync(userRegister.Email) != null)
        {
            throw new BadRequestException($"Email {userRegister.Email} already exists.");
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
                throw new BadRequestException($"Error registering user role {roleAssigning}.");

            var userResponse = new UserResponse()
            {
                Id = user.Id,
                Email = user.Email,
                Roles = [RoleConstants.User],
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };
            
            return GenerateToken(userResponse);
        }
        
        throw new BadRequestException($"Error registering user : {createUserResult.ToString()}.");
    }

    public async Task<UserResponse> Login(UserLogin userLogin)
    {
        var user = await userManager.FindByNameAsync(userLogin.UserName);
        
        if (user == null)
            throw new BadRequestException($"User {userLogin.UserName} not found.");
        
        var checkPasswordResult = await userManager.CheckPasswordAsync(user, userLogin.Password);

        if (checkPasswordResult == false)
        {
            throw new BadRequestException("Invalid password.");
        }
        
        var userRoles = await userManager.GetRolesAsync(user);
        var userResponse = new UserResponse()
        {
            Id          = user.Id,
            Email       = user.Email,
            Roles       = userRoles,
            UserName    = user.UserName,
            PhoneNumber = user.PhoneNumber
        };
        return GenerateToken(userResponse);
    }

    public async Task SetUserRole(int userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
            throw new BadRequestException($"User {userId} not found.");
        
        if (role != RoleConstants.User || role != RoleConstants.Admin || role != RoleConstants.Merchant)
            throw new BadRequestException("Invalid role.");
        
        var res = await userManager.AddToRoleAsync(user, role.ToString());
    }

    private UserResponse GenerateToken(UserResponse userResponse)
    {
        var handler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_authOptions.TokenPrivateKey);
        var creds = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject            = GenerateClaims(userResponse),
            Expires            = DateTime.UtcNow.AddMinutes(_authOptions.ExpireIntervalInMinutes),
            SigningCredentials = creds
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        userResponse.Token = handler.WriteToken(token);
        
        return userResponse;
    }

    private ClaimsIdentity GenerateClaims(UserResponse userResponse)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, userResponse.UserName));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userResponse.Id.ToString()));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, "test"));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, "test"));
        //claims.AddClaim(new Claim());

        foreach (var role in userResponse.Roles!)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        
        return claims;
    }
}