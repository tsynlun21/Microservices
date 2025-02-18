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
        if (response == null)
        {
            await context.RespondAsync(new BaseMasstransitResponse()
            {
                Success = false,
                Message = $"Transaction with vehicle with VIN {context.Message.Transaction.Vehicle.Vin} already exists. "
            });
        }
        else
        {
            await context.RespondAsync(new BaseMasstransitResponse()
            {
                Success = true,
                Message = $"Transaction added successfully."
            });
        }
    }
}