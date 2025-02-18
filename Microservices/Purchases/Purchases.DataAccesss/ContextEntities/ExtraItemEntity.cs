using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Showrooms.Enums;

namespace Purchases.DataAccesss.ContextEntities;

public class ExtraItemEntity
{
    [Key]
    public long Id { get; set; }

    [ForeignKey("TransactionKey")]
    public TransactionEntity Transaction { get; set; } = null!;

    public long TransactionKey { get; set; }

    public string Name { get; set; }

    public string VehicleModel { get; set; }

    public int Count { get; set; }

    public ExtraItemType Type { get; set; }

    public decimal Price { get; set; }
}