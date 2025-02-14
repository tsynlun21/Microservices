namespace Infrastructure.Models.Showrooms;

public class VehiclesPerShowroom
{
    public long ShowroomId { get; set; }
    public List<Vehicle> Vehicles { get; set; }
}