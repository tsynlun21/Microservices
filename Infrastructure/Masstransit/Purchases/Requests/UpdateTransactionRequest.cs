using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;

namespace Infrastructure.Masstransit.Purchases.Requests;

public class UpdateTransactionRequest
{
    public User User { get; set; }
    public UpdateTransaction Transaction { get; set; }
}