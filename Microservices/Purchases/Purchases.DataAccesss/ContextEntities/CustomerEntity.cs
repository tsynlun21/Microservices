using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Purchases.DataAccesss.ContextEntities;

[Index(nameof(UserName), IsUnique = true)]
public class CustomerEntity
{
    [Key]
    public long Id { get; set; }
    public string CustomerId { get; set; }
    public List<TransactionEntity> Transactions { get; set; } = new();
    public string UserName { get; set; }
}