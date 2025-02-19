using CarHistory.Domain.Services;
using Infrastructure.Exceptions;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.CarHistory.Requests;
using MassTransit;

namespace CarHistory.API.Consumers;

public class AddCarHistoryConsumer(ICarHistoryService service) : IConsumer<AddCarHistoryRequest>
{
    public async Task Consume(ConsumeContext<AddCarHistoryRequest> context)
    {
        var res = await service.AddCarHistoryAsync(context.Message.CarHistories); 
        await context.RespondAsync(res.ToArray());   
    }
}