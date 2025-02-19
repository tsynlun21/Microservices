using CarHistory.Domain.Services;
using Infrastructure.Exceptions;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.CarHistory;
using Infrastructure.Masstransit.CarHistory.Requests;
using MassTransit;

namespace CarHistory.API.Consumers;

public class GetCarHistoryConsumer(ICarHistoryService service) : IConsumer<GetCarHistoryRequest>
{
    public async Task Consume(ConsumeContext<GetCarHistoryRequest> context)
    {
        var res = await service.GetCarHistoryAsync(context.Message.Vin);
        
        await context.RespondAsync(res);
    }
}