using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Purchases.Requests;
using Infrastructure.Masstransit.Showrooms.Requests;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class BuyVehicleConsumer(IShowroomService service, IBus bus) : IConsumer<BuyVehicleMessageRequest>
{
    private readonly Uri rabbitPurchasesUrl = new Uri($"queue:{RabbitQueueNames.PURCHASES}");
    
    public async Task Consume(ConsumeContext<BuyVehicleMessageRequest> context)
    {
        var purchase = await service.BuyVehicle(context.Message.ShowroomId, context.Message.Purchase);
        await context.RespondAsync(purchase);
        
        var transaction = await service.CreateTransaction(context.Message.ShowroomId, purchase);
        await service.AddReceipt(transaction.Receipt);

        var endpoint = await bus.GetSendEndpoint(rabbitPurchasesUrl);
        await endpoint.Send(new AddTransactionRequest
        {
            User = context.Message.User,
            Transaction = transaction
        });
    }
}