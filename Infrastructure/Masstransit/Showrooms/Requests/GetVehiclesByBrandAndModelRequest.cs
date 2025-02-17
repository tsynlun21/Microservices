using System.ComponentModel.DataAnnotations;

namespace CarsShowroom.Domain.Requests;

public class GetVehiclesByBrandAndModelRequest
{
    [Required]
    public long ShowroomId { get; set; }
    
    [Required]
    public string Brand { get; set; }
    
    public string Model { get; set; }
}