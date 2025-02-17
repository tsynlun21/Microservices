using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Purchases.DataAccesss.ContextEntities;

public class ReceiptEntity
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey("TransactionEntityKey")]
    public TransactionEntity Transaction { get; set; }
    public long TransactionEntityKey { get; set; }
    
    public long ShowroomId { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public int Quantity { get; set; }
    
    public DateTime Date { get; set; }
}