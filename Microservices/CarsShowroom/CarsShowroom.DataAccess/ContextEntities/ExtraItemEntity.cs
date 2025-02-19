using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Showrooms.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarsShowroom.DataAccess.ContextEntities;

[Table("ExtraItems")]
[Index(nameof(Name), nameof(VehicleModelEntityKey), nameof(Type), IsUnique = true)]
[Index(nameof(Name), nameof(VehicleModelEntityKey), IsUnique = true)]
public class ExtraItemEntity
{
    [Key]
    public long Id { get; set; }
    
    public string Name { get; set; }
    public ExtraPartType Type { get; set; }
    
    [ForeignKey("VehicleModelEntityKey")]
    public VehicleModelEntity VehicleModelEntity { get; set; }
    
    public long VehicleModelEntityKey { get; set; }
    
    public int Count { get; set; }
    
    public decimal Price { get; set; }
}