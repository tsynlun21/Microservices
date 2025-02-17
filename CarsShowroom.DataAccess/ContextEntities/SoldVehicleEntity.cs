using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Showrooms.Enums;

namespace CarsShowroom.DataAccess.ContextEntities;

public class SoldVehicleEntity
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("ReceiptKey")]
    public ReceiptEntity Receipt { get; set; }
    public long ReceiptKey { get; set; }
    
    public string Brand { get; set; }
    public string Model { get; set; }
    public LPCColors Color { get; set; }
    public decimal Price { get; set; }
    public string Vin { get; set; }
    public DateOnly ReleaseDate { get; set; }
}