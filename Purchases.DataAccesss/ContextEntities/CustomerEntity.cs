using System.ComponentModel.DataAnnotations;

namespace Purchases.DataAccesss.ContextEntities;

public class CustomerEntity
{
    [Key]
    public long Id { get; set; }
    public string CustomerId { get; set; }
    public List<TransactionEntity> Transactions { get; set; } = new();
}