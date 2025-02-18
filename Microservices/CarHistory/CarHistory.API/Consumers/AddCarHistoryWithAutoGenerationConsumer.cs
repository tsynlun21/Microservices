using CarHistory.Domain.Services;
using Infrastructure.Masstransit.CarHistory.Requests;
using MassTransit;

namespace CarHistory.API.Consumers;

public class AddCarHistoryWithAutoGenerationConsumer(ICarHistoryService service) : IConsumer<AddCarHistoryWithGenerationRequest>
{
    public async Task Consume(ConsumeContext<AddCarHistoryWithGenerationRequest> context)
    {
        await service.AddCarHistoryWithGenerationAsync(context.Message.CarHistories);
    }
}