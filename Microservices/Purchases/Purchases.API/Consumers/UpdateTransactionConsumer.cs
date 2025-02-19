using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Purchases.Requests;
using MassTransit;
using Purchases.Domain.Services.Interfaces;

namespace Purchases.API.Consumers;

public class UpdateTransactionConsumer(IPurchasesService service) : IConsumer<UpdateTransactionRequest>
{
    public async Task Consume(ConsumeContext<UpdateTransactionRequest> context)
    {
        var res = await service.UpdateTransaction(context.Message.User, context.Message.Transaction);
        await context.RespondAsync(res);
    }
}