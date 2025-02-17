using Infrastructure.Models.Showrooms;

namespace Infrastructure.Masstransit.Showrooms.Requests;

public class AddVehiclesRequest
{
    public ICollection<VehiclesPerShowroom> VehiclesPerShowrooms { get; set; }
}