using Infrastructure.Models.Identity;

namespace Infrastructure.Masstransit.Purchases.Requests;

public class GetTransactionByIdRequest
{
    public User User { get; set; }
    public long TransactionId { get; set; }
}