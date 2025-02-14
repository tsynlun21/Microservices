using CarsShowroom.Domain.Requests;
using CarsShowroom.Domain.Response;
using Infrastructure.Models.Purchases;
using Infrastructure.Models.Showrooms;

namespace CarsShowroom.Domain.Services.Interfaces;

public interface IShowroomService
{
    Task<ICollection<Vehicle>> GetAllVehiclesByShowroom(long showroomId);
    Task<ICollection<Vehicle>> GetAllVehiclesByBrandAndModel(BrandAndModelRequest request);
    Task<ICollection<VehiclesForShowroom>> GetAllVehiclesInPrice(PriceLimitRequest request);
    Task<ICollection<ExtraItem>> GetAllExtrasByModelName(string modelName);

    Task AddVehiclesToShowroom(ICollection<VehiclesPerShowroom> vehicles);
    Task AddExtraItemsToVehicleInShowroom(ICollection<ExtraItemToVehicle> vehicles);

    Task<ICollection<Vehicle>> BuyVehicle(long showroomId, ICollection<Vehicle> vehicles);
    Task<Transaction> CreateTransaction(int showroomId, ICollection<Vehicle> vehicles);

    Task AddReceipt(Receipt receipt);
}