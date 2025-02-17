using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Purchases.Enums;

namespace Purchases.DataAccesss.ContextEntities;

public class TransactionEntity
{
    [Key]
    public long Id { get; set; }

    [ForeignKey("CustomerEntityKey")]
    public CustomerEntity CustomerEntity { get; set; }

    public long CustomerEntityKey { get; set; }

    public ReceiptEntity Receipt { get; set; }

    public VehicleEntity Vehicle { get; set; }
    public ICollection<ExtraItemEntity> ExtraItems { get; set; }

    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public bool IsPerfomedByShowroom { get; set; }
}