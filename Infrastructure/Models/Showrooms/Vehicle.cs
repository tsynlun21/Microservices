using System.Text.Json.Serialization;
using Infrastructure.Models.Showrooms.Enums;

namespace Infrastructure.Models.Showrooms;

public class Vehicle
{
    public long VehicleId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateTime ReleaseDate { get; set; }
    public LPCColors Color { get; set; }
    public decimal Price { get; set; }
    public string Vin { get; set; }
    
    [JsonIgnore]
    public List<ExtraItem> ExtraItems { get; set; }
    
}
