using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.Showrooms.Requests;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class GetExtraPartsConsumer(IShowroomService service) : IConsumer<GetExtraPartsRequest>
{
    public async Task Consume(ConsumeContext<GetExtraPartsRequest> context)
    {
        var res = await service.GetAllExtrasByModelName(context.Message.Model);

        await context.RespondAsync(res.ToArray());
    }
}