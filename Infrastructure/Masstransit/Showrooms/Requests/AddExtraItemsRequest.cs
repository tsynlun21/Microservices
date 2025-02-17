using Infrastructure.Models.Showrooms;

namespace Infrastructure.Masstransit.Showrooms.Requests;

public class AddExtraItemsRequest
{
    public ICollection<ExtraItem> ExtraItems { get; set; }
}