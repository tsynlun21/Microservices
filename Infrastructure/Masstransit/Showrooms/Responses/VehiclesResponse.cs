using Infrastructure.Models.Showrooms;

namespace Infrastructure.Masstransit.Showrooms.Responses;

public class VehiclesResponse
{
    public ICollection<Vehicle> Vehicles { get; set; }
}