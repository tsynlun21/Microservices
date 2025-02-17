namespace Infrastructure.Masstransit.Identity.Requests;

public class SetUserRoleRequest
{
    public int UserId { get; set; }
    public string Role { get; set; }
}