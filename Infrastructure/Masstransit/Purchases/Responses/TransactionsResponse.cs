using Infrastructure.Models.Purchases;

namespace Infrastructure.Masstransit.Purchases.Responses;

public class TransactionsResponse
{
    public ICollection<Transaction> Transactions { get; set; }
}