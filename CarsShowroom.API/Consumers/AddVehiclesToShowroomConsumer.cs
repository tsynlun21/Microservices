using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Masstransit.Showrooms.Responses;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class AddVehiclesToShowroomConsumer(IShowroomService service) : IConsumer<AddVehiclesRequest>
{
    public async Task Consume(ConsumeContext<AddVehiclesRequest> context)
    {
        var res = await service.AddVehiclesToShowroom(context.Message.VehiclesPerShowrooms);

        await context.RespondAsync(new VehiclesResponse()
        {
            Vehicles = res
        });
    }
}