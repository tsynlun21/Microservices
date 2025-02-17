using Infrastructure.Models.Showrooms;

namespace Infrastructure.Masstransit.Showrooms.Responses;

public class VehiclesInPriceForShowroom
{
    public string Adress { get; set; } = string.Empty;
    public string ContactNumber { get; set; }
    
    public List<Vehicle> Vehicles { get; set; }
}

public class VehicleInPriceResponse
{
    public ICollection<VehiclesInPriceForShowroom> VehiclesInPrice { get; set; }
}