using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Showrooms.Enums;

namespace CarsShowroom.DataAccess.ContextEntities;

[Table("ExtraItems")]
public class ExtraItemEntity
{
    [Key]
    public long Id { get; set; }
    
    public string Name { get; set; }
    public ExtraItemType Type { get; set; }
    
    [ForeignKey("VehicleModelEntityKey")]
    public VehicleModelEntity VehicleModelEntity { get; set; }
    
    public long VehicleModelEntityKey { get; set; }
    
    public int Count { get; set; }
}