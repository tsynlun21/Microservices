using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;

namespace Infrastructure.Masstransit.Purchases.Requests;

public class AddTransactionRequest
{
    public UserModel User { get; set; }
    public Transaction Transaction { get; set; }
}