namespace Infrastructure.Models.Identity;

public class UserResponse
{
    public string Id { get; set; }
    public ICollection<string>? Roles { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Token { get; set; }
}