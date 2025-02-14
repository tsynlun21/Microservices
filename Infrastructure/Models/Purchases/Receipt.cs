using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infrastructure.Models.Showrooms;

namespace Infrastructure.Models.Purchases;

public class Receipt
{
    [Required]
    public long ShoowRoomId { get; set; }
    
    [Required]
    public decimal TotalPrice { get; set; }
    
    [Required]
    public int TotalQuantity { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [JsonIgnore]
    public List<Vehicle> Vehicles { get; set; }
    
    [JsonIgnore]
    public List<ExtraItem> ExtraItems { get; set; }
}