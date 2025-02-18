using Infrastructure.Masstransit.Purchases.Requests;
using MassTransit;
using Purchases.Domain.Services.Interfaces;

namespace Purchases.API.Consumers;

public class GetTransactionByIdConsumer(IPurchasesService service) : IConsumer<GetTransactionByIdRequest>
{
    public async Task Consume(ConsumeContext<GetTransactionByIdRequest> context)
    {
        var res = await service.GetTransactionById(context.Message.User, context.Message.TransactionId);
        
        await context.RespondAsync(res);
    }
}