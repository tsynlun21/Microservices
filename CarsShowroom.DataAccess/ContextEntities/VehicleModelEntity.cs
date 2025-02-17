using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CarsShowroom.DataAccess.ContextEntities;

[Index(nameof(Model), IsUnique = true)]
public class VehicleModelEntity
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public string Model { get; set; }
    
    public List<ExtraItemEntity> ExtraItems { get; set; }
}