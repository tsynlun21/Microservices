using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.CarHistory.Requests;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Masstransit.Showrooms.Responses;
using Infrastructure.Models.CarHistory;
using Infrastructure.Models.Showrooms;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class AddVehiclesToShowroomConsumer(IShowroomService service) : IConsumer<AddVehiclesRequest>
{
    public async Task Consume(ConsumeContext<AddVehiclesRequest> context)
    {
        var carHistoryRequest = new AddCarHistoryWithGenerationRequest()
        {
            CarHistories = CreateCarHistoryRequestData(context.Message.VehiclesPerShowrooms)
        };
        
        await context.Publish(carHistoryRequest);
        
        var res = await service.AddVehiclesToShowroom(context.Message.VehiclesPerShowrooms);
        
        await context.RespondAsync(new VehiclesResponse()
        {
            Vehicles = res
        });
    }

    private ICollection<CarHistoryBase> CreateCarHistoryRequestData(ICollection<VehiclesPerShowroom> incomingCollection)
    {
        var allVehicles = incomingCollection.SelectMany(x => x.Vehicles).ToList();
        
        var carHistoryData = allVehicles.Select(ToCarHistoryBase).ToList();
        
        return carHistoryData;
    }

    private CarHistoryBase ToCarHistoryBase(Vehicle vehicle)
    {
        return new CarHistoryBase()
        {
            Vin         = vehicle.Vin,
            Brand       = vehicle.Brand,
            Model       = vehicle.Model,
            Color       = vehicle.Color.ToString(),
            Mileage     = vehicle.Mileage,
            ReleaseDate = vehicle.ReleaseDate,
        };
    }
}