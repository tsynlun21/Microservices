using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Purchases.Requests;
using MassTransit;
using Purchases.Domain.Services.Interfaces;

namespace Purchases.API.Consumers;

public class AddTransactionConsumer(IPurchasesService service) : IConsumer<AddTransactionRequest>
{
    public async Task Consume(ConsumeContext<AddTransactionRequest> context)
    {
        var response = await service.AddTransaction(context.Message.User, context.Message.Transaction);
        
        await context.RespondAsync(response);
    }
}