using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Masstransit.Showrooms.Requests;

public class PriceLimitRequest
{
    public decimal? MaxPrice { get; set; }
    
    [Required]
    public decimal MinPrice { get; set; }
}