namespace Infrastructure.Models.Identity;

public class UserModel
{
    public string Id { get; set; }
    public ICollection<string>? Roles { get; set; }
    public string? Email { get; set; }
    public required string UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Token { get; set; }
}