using Infrastructure.Models.Showrooms;

namespace Infrastructure.Models.Purchases;

public class Purchase
{
    public Vehicle Vehicle { get; set; }
    
    public ICollection<ExtraItem> ExtraItems { get; set; }
}

public class PurchaseOrder
{
    public long VehicleId { get; set; }
    
    public ICollection<ExtraItemOrder> ExtraItems { get; set; }
}

