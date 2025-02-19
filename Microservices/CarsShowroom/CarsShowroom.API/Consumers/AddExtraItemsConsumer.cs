using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.Showrooms.Requests;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class AddExtraItemsConsumer(IShowroomService service) : IConsumer<AddExtraPartsRequest>
{
    public async Task Consume(ConsumeContext<AddExtraPartsRequest> context)
    {
        var res = await service.AddExtraItems(context.Message.ExtraItems);
        
        await context.RespondAsync(res.ToArray());
    }
}