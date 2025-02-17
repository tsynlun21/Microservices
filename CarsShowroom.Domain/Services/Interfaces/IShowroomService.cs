using CarsShowroom.Domain.Requests;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Masstransit.Showrooms.Responses;
using Infrastructure.Models.Purchases;
using Infrastructure.Models.Showrooms;

namespace CarsShowroom.Domain.Services.Interfaces;

public interface IShowroomService
{
    Task<ICollection<Vehicle>> GetAllVehiclesByShowroom(long showroomId);
    Task<ICollection<Vehicle>> GetAllVehiclesByBrandAndModel(GetVehiclesByBrandAndModelRequest request);
    Task<ICollection<VehiclesInPriceForShowroom>> GetAllVehiclesInPrice(PriceLimitRequest request);
    Task<ICollection<ExtraItem>> GetAllExtrasByModelName(string modelName);

    Task<ICollection<Vehicle>> AddVehiclesToShowroom(ICollection<VehiclesPerShowroom> vehicles);
    Task<ICollection<ExtraItem>> AddExtraItems(ICollection<ExtraItem> extraItems);

    Task<Purchase> BuyVehicle(long showroomId, PurchaseOrder purchaseOrder);
    Task<Transaction> CreateTransaction(long showroomId, Purchase purchase);

    Task AddReceipt(Receipt receipt);
}