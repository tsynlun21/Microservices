using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsShowroom.DataAccess.ContextEntities;

[Table("Showrooms")]
public class ShowroomEntity
{
    [Key]
    public long Id { get; set; }  
    
    public string Address { get; set; }
    
    [Phone]
    public string ContactNumber { get; set; }
    
    public List<VehicleEntity> Vehicles { get; set; }
    public List<ExtraItemEntity> ExtraItems { get; set; }
    public List<ReceiptEntity> Receipts { get; set; }
}