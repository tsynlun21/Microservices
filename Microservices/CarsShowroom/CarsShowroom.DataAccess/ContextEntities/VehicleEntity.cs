using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Showrooms.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarsShowroom.DataAccess.ContextEntities;

[Table("Vehicles")]
[Index(nameof(Brand), nameof(VehicleModelId), nameof(Color), nameof(Vin),IsUnique = true)]
public class VehicleEntity
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey("ShowroomEntityKey")]
    public ShowroomEntity Showroom { get; set; }
    public long ShowroomEntityKey { get; set; }
    
    public string Vin { get; set; }
    public string Brand { get; set; }
    public decimal Price { get; set; }
    public decimal Mileage { get; set; }
    
    [ForeignKey("VehicleModelId")]
    public VehicleModelEntity VehicleModelEntity { get; set; }
    public long VehicleModelId { get; set; }       
    public LPCColors Color { get; set; }
    public DateOnly ReleaseDate { get; set; }
}