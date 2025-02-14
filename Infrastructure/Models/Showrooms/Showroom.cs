using System.Text.Json.Serialization;

namespace Infrastructure.Models.Showrooms;

public class Showroom
{
    public long Id { get; set; }
    
    public string Address { get; set; }
    
    public string ContactNumber { get; set; }
    
    public List<Vehicle> Vehicles { get; set; }
    
    [JsonIgnore]
    public List<ExtraItem> ExtraItems { get; set; }
}