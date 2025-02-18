using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Showrooms.Enums;

namespace Purchases.DataAccesss.ContextEntities;

public class VehicleEntity
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey("TransactionKey")]
    public TransactionEntity Transaction { get; set; }
    public long TransactionKey { get; set; }
    
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public LPCColors Color { get; set; }
    public decimal Price { get; set; }
    public string Vin { get; set; }
}