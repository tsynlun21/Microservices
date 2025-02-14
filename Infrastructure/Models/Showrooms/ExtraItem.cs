using Infrastructure.Models.Showrooms.Enums;

namespace Infrastructure.Models.Showrooms;

public class ExtraItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    public ExtraItemType Type { get; set; }
    public long VehicleModel { get; set; }
    public int Count { get; set; }
}