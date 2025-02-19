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
    
    [Required]
    public Vehicle Vehicle { get; set; }
    
    [JsonIgnore]
    public List<ExtraPart>? ExtraItems { get; set; }
}