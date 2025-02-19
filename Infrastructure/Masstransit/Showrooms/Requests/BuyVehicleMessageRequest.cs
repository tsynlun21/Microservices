using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;
using MassTransit.Futures.Contracts;

namespace Infrastructure.Masstransit.Showrooms.Requests;

public class BuyVehicleMessageRequest : BuyVehicleRequest
{
    public UserModel User { get; set; }
}

public class BuyVehicleRequest
{
    public long ShowroomId { get; set; }
    public PurchaseOrder Purchase { get; set; }
}