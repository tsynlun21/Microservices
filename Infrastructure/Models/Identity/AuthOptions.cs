namespace Infrastructure.Models.Identity;

public class AuthOptions
{
    public required string TokenPrivateKey { get; set; }
    public int ExpireIntervalInMinutes { get; set; }
}