namespace Infrastructure.Models.Showrooms;

public class ExtraItemToVehicle
{
    public long VehicleId { get; set; }
    public long ExtraItemId { get; set; }
    public long Quantity { get; set; }
}