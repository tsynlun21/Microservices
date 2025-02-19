using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Showrooms.Enums;

namespace CarsShowroom.DataAccess.ContextEntities;

public class SoldExtraItemsEntity
{
    [Key]
    public long Id { get; set; }

    public string Name { get; set; }
    
    public ExtraPartType Type { get; set; }
    
    [ForeignKey("VehicleModelKey")]
    public VehicleModelEntity VehicleModel { get; set; }
    public string VehicleModelKey { get; set; }
    
    public int Count { get; set; }
    
    public decimal Price { get; set; }
}