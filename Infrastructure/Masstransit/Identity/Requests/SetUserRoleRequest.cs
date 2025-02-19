using System.ComponentModel.DataAnnotations;
using Infrastructure.Exceptions;
using Infrastructure.Models.Identity;

namespace Infrastructure.Masstransit.Identity.Requests;

public class SetUserRoleRequest
{
    public string UserId { get; set; }
    
    [RoleValidation]
    public string Role { get; set; }
}

public class RoleValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (RoleConstants.Roles.Contains(value?.ToString()) is false)
            throw new BadRequestException($"Role {value?.ToString()} is not valid");
        
        return true;
    }
}