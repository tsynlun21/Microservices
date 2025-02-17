using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Masstransit.Showrooms.Responses;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class GetExtraPartsConsumer(IShowroomService service) : IConsumer<GetExtraPartsRequest>
{
    public async Task Consume(ConsumeContext<GetExtraPartsRequest> context)
    {
        var res = await service.GetAllExtrasByModelName(context.Message.Model);

        await context.RespondAsync(new ExtraItemsResponse
        {
            ExtraItems = res
        });
    }
}