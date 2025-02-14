using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsShowroom.DataAccess.ContextEntities;

[Table("Receipts")]
public class ReceiptEntity
{
    [Key]
    public long Id { get; set; }  
    
    [ForeignKey("ShowroomEntityKey")]
    public ShowroomEntity Showroom { get; set; }
    public int ShoowRoomEntityKey { get; set; }
    
    public List<VehicleEntity> Vehicles { get; set; }
    public List<ExtraItemEntity> ExtraItems { get; set; }
    
    public decimal TotalPrice { get; set; }
    public int TotalItems { get; set; }
}