using Infrastructure.Models.Identity;

namespace Infrastructure.Masstransit.Purchases.Requests;

public class GetTransactionsRequest
{
    public User user { get; set; }
}