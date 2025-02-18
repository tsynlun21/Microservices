using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Masstransit.Showrooms.Responses;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class GetVehiclesByShowRoomIdConsumer(IShowroomService showroomService) : IConsumer<GetVehiclesByShowRoomIdRequest>
{
    public async Task Consume(ConsumeContext<GetVehiclesByShowRoomIdRequest> context)
    {
        var res = await showroomService.GetAllVehiclesByShowroom(context.Message.ShowRoomId);
        await context.RespondAsync(new VehiclesResponse
        {
            Vehicles   = res 
        });
    }
}