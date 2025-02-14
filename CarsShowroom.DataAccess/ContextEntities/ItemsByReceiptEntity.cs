using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsShowroom.DataAccess.ContextEntities;

[Table("ItemsByReceipt")]
public class ItemsByReceiptEntity
{
    [Key]
    public long Id { get; set; }  
    
    [ForeignKey("ReceiptEntityKey")]
    public ReceiptEntity Receipt { get; set; }
    
    public long ReceiptEntityKey { get; set; }
    
    public long ItemsAmount { get; set; }
    
    public string Name { get; set; }
    
    public decimal TotalPrice { get; set; }
}