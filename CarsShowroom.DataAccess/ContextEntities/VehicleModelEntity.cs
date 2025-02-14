using System.ComponentModel.DataAnnotations;

namespace CarsShowroom.DataAccess.ContextEntities;

public class VehicleModelEntity
{
    [Key]
    public long Id { get; set; }
    
    public string Model { get; set; }
    
    public List<ExtraItemEntity> ExtraItems { get; set; }
}