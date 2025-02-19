using Infrastructure.Models.Showrooms;

namespace Infrastructure.Models.Purchases;

public class Purchase
{
    public Vehicle Vehicle { get; set; }
    
    public ICollection<ExtraPart> ExtraItems { get; set; }
}

public class PurchaseOrder
{
    public long VehicleId { get; set; }
    
    public ICollection<ExtraPartOrder> ExtraItems { get; set; }
}

