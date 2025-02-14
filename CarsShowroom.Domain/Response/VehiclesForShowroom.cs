using Infrastructure.Models.Showrooms;

namespace CarsShowroom.Domain.Response;

public class VehiclesForShowroom
{
    public string Adress { get; set; } = string.Empty;
    public string ContactNumber { get; set; }
    
    public List<Vehicle> Vehicles { get; set; }
}