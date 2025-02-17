using Infrastructure.Models.Showrooms;

namespace Infrastructure.Masstransit.Showrooms.Responses;

public class ExtraItemsResponse
{
    public ICollection<ExtraItem> ExtraItems { get; set; }
}