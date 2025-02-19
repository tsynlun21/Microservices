using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.EntititesContext;

public class IdentityRoleEntity : IdentityRole<string>
{
}