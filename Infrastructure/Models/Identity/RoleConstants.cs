namespace Infrastructure.Models.Identity;

public class RoleConstants
{
    public const string Admin = "admin";
    public const string Merchant = "merchant";
    public const string User = "user";
    
    public readonly static string[] Roles = { Admin, Merchant, User };
}