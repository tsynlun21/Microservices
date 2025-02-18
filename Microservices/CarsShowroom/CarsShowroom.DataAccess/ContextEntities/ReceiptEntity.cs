using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsShowroom.DataAccess.ContextEntities;

[Table("Receipts")]
public class ReceiptEntity
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey("ShowroomKey")]
    public ShowroomEntity Showroom { get; set; }
    public long ShowroomKey { get; set; }
    
    public List<SoldExtraItemsEntity> SoldExtraItems { get; set; }
    
    public SoldVehicleEntity SoldVehicle { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public int TotalItemsCount { get; set; }
}