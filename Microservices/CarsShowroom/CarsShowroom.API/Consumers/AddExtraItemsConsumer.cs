using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Masstransit.Showrooms.Responses;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class AddExtraItemsConsumer(IShowroomService service) : IConsumer<AddExtraItemsRequest>
{
    public async Task Consume(ConsumeContext<AddExtraItemsRequest> context)
    {
        var res = await service.AddExtraItems(context.Message.ExtraItems);
        
        await context.RespondAsync(new ExtraItemsResponse()
        {
            ExtraItems = res
        });
    }
}