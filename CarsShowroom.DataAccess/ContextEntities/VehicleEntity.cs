using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Showrooms.Enums;

namespace CarsShowroom.DataAccess.ContextEntities;

[Table("Vehicles")]
public class VehicleEntity
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey("ShowroomEntityKey")]
    public ShowroomEntity Showroom { get; set; }
    public long ShowroomEntityKey { get; set; }
    
    public string Uid { get; set; }
    public string Brand { get; set; }
    public decimal Price { get; set; }
    
    [ForeignKey("VehicleModelId")]
    public VehicleModelEntity VehicleModelEntity { get; set; }
    public long VehicleModelId { get; set; }       
    public LPCColors Color { get; set; }
    public DateTime ReleaseDate { get; set; }
}