using Infrastructure.Masstransit.Purchases.Requests;
using MassTransit;
using Purchases.Domain.Services.Interfaces;

namespace Purchases.API.Consumers;

public class GetTransactionConsumer(IPurchasesService service) : IConsumer<GetTransactionsRequest>
{
    public async Task Consume(ConsumeContext<GetTransactionsRequest> context)
    {
        var response = await service.GetTransactions(context.Message.user);
        
        await context.RespondAsync(response.ToArray());
    }
}