using System.ComponentModel.DataAnnotations;

namespace CarsShowroom.Domain.Requests;

public class PriceLimitRequest
{
    public decimal? MaxPrice { get; set; }
    
    [Required]
    public decimal MinPrice { get; set; }
}