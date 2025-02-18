namespace Infrastructure.Masstransit.CarHistory.Requests;

public class AddCarHistoryRequest
{
    public ICollection<Models.CarHistory.CarHistory> CarHistories { get; set; }
}