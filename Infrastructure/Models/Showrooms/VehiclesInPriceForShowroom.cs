using Infrastructure.Models.Showrooms;

namespace CarsShowroom.Domain.Services;

public class VehiclesInPriceForShowroom
{
    public string Adress { get; set; }
    public string ContactNumber { get; set; }
    
    public ICollection<Vehicle> Vehicles { get; set; }
}