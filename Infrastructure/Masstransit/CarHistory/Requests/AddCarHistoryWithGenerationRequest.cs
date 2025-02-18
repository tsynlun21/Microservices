using Infrastructure.Models.CarHistory;

namespace Infrastructure.Masstransit.CarHistory.Requests;

public class AddCarHistoryWithGenerationRequest
{
    public ICollection<CarHistoryBase> CarHistories { get; set; }
}