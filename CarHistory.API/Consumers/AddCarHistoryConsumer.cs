using CarHistory.Domain.Services;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.CarHistory.Requests;
using MassTransit;

namespace CarHistory.API.Consumers;

public class AddCarHistoryConsumer(ICarHistoryService service) : IConsumer<AddCarHistoryRequest>
{
    public async Task Consume(ConsumeContext<AddCarHistoryRequest> context)
    {
        await service.AddCarHistoryAsync(context.Message.CarHistories);

        await context.RespondAsync(new BaseMasstransitResponse()
        {
            Success = true,
            Message = "Successfully added car history",
        });
    }
}