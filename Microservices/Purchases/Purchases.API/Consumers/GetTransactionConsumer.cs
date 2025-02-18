using Infrastructure.Masstransit.Purchases.Requests;
using Infrastructure.Masstransit.Purchases.Responses;
using MassTransit;
using Purchases.Domain.Services.Interfaces;

namespace Purchases.API.Consumers;

public class GetTransactionConsumer(IPurchasesService service) : IConsumer<GetTransactionsRequest>
{
    public async Task Consume(ConsumeContext<GetTransactionsRequest> context)
    {
        var response = await service.GetTransactions(context.Message.user);
        
        await context.RespondAsync(new TransactionsResponse()
        {
            Transactions = response
        });
    }
}