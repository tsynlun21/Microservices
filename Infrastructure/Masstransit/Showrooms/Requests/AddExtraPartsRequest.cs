using Infrastructure.Models.Showrooms;

namespace Infrastructure.Masstransit.Showrooms.Requests;

public class AddExtraPartsRequest
{
    public ICollection<ExtraItem> ExtraItems { get; set; }
}