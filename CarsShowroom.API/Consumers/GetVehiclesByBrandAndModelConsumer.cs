using CarsShowroom.Domain.Requests;
using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.Showrooms.Responses;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class GetVehiclesByBrandAndModelConsumer(IShowroomService service) : IConsumer<GetVehiclesByBrandAndModelRequest>
{
    public async Task Consume(ConsumeContext<GetVehiclesByBrandAndModelRequest> context)
    {
        var res = await service.GetAllVehiclesByBrandAndModel(context.Message);
        await context.RespondAsync(new VehiclesResponse()
        {
            Vehicles = res
        });
    }
}